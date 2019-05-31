using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace GCAL.Base.Scripting
{
    public class GSExecutor: GSCore
    {
        private List<Dictionary<string, GSCore>> stackVars = new List<Dictionary<string, GSCore>>();

        private StringBuilder output = new StringBuilder();
        private string markupStart = "[";
        private string markupEnd = "]";
        private string escapeChar = "\\";

        public GSExecutor()
        {
            stackVars.Add(new Dictionary<string, GSCore>());
        }

        public override GSCore ExecuteMessage(string token, GSCoreCollection args)
        {
            GSCore result = null;
            if (token.Equals("add") || token.Equals("+"))
                result = execAdd(getNativeValues(args));
            else if (token.Equals("sub") || token.Equals("-"))
                result = execSub(getNativeValues(args));
            else if (token.Equals("mul") || token.Equals("*"))
                result = execMul(getNativeValues(args));
            else if (token.Equals("div") || token.Equals("/"))
                result = execDiv(getNativeValues(args));
            else if (token.Equals("and") || token.Equals("&"))
                result = execAnd(getNativeValues(args));
            else if (token.Equals("or") || token.Equals("|"))
                result = execOr(getNativeValues(args));
            else if (token.Equals("not") || token.Equals("!"))
                result = execNot(args);
            else if (token.Equals("set") && args.Count > 1)
                result = execSet(args[0], args[1]);
            else if ((token.Equals("gt") || token.Equals(">")) && args.Count > 1)
                result = execGt(getNativeValues(args));
            else if ((token.Equals("ge") || token.Equals(">=")) && args.Count > 1)
                result = execGe(getNativeValues(args));
            else if ((token.Equals("eq") || token.Equals("==")) && args.Count > 1)
                result = execEq(getNativeValues(args));
            else if ((token.Equals("ne") || token.Equals("!=")) && args.Count > 1)
                result = execNe(getNativeValues(args));
            else if ((token.Equals("le") || token.Equals("<=")) && args.Count > 1)
                result = execLe(getNativeValues(args));
            else if ((token.Equals("lt") || token.Equals("<")) && args.Count > 1)
                result = execLt(getNativeValues(args));
            else if (token.Equals("print"))
                result = execPrint(args, false);
            else if (token.Equals("println"))
                result = execPrint(args, true);
            else if (token.Equals("if"))
                result = execIf(args);
            else if (token.Equals("while"))
                result = execWhile(args);
            else if (token.Equals("foreach"))
                result = execForeach(args);
            else if (token.Equals("x"))
                result = execMessage(args);
            else if (token.Equals("do"))
                result = execDo(args);
            else if (token.Equals("return"))
                result = new GSReturn(args.getSafe(0));
            else if (token.Equals("break"))
                result = new GSReturn(GSReturn.TYPE_BREAK);
            else if (token.Equals("continue"))
                result = new GSReturn(GSReturn.TYPE_CONTINUE);
            else
            {
                Debugger.Log(0, "", "UNKNOWN MESSAGE: " + token + " ");
            }

            return result;
        }

        //
        // Methods that are helpful for execution of the program
        //
        #region Helper Methods

        public GSCoreCollection getNativeValues(GSCoreCollection args)
        {
            GSCoreCollection coll = new GSCoreCollection();
            foreach (GSCore item in args)
            {
                coll.Add(ExecuteElement(item));
            }
            return coll;
        }

        private Dictionary<string, GSCore> getLastVars()
        {
            return stackVars[stackVars.Count - 1];
        }

        public long[] getIntegerArray(GSCoreCollection C)
        {
            long[] result = new long[C.Count];
            for (int i = 0; i < C.Count; i++)
            {
                result[i] = ExecuteElement(C[i]).getIntegerValue();
            }
            return result;
        }
        public double[] getDoubleArray(GSCoreCollection C)
        {
            double[] result = new double[C.Count];
            for (int i = 0; i < C.Count; i++)
            {
                result[i] = ExecuteElement(C[i]).getDoubleValue();
            }
            return result;
        }
        public string[] getStringArray(GSCoreCollection C)
        {
            string[] result = new string[C.Count];
            for (int i = 0; i < C.Count; i++)
            {
                result[i] = ExecuteElement(C[i]).getStringValue();
            }
            return result;
        }
        public bool[] getBooleanArray(GSCoreCollection C)
        {
            bool[] result = new bool[C.Count];
            for (int i = 0; i < C.Count; i++)
            {
                result[i] = ExecuteElement(C[i]).getBooleanValue();
            }
            return result;
        }

        public string ReplaceVariables(string str)
        {
            int lastPut = 0;
            int idx = 0;
            int ide = 0;
            StringBuilder sb = new StringBuilder();

            while ((idx = FindVariablePlaceholder(str, idx)) >= 0)
            {
                ide = FindVariablePlaceholderEnd(str, idx);
                if (ide >= idx)
                {
                    sb.Append(str.Substring(lastPut, idx - markupStart.Length - lastPut));
                    string ph = str.Substring(idx, ide - idx);
                    sb.Append(ReplaceVariablePlaceholder(ph));
                    lastPut = ide + markupEnd.Length;
                }
                else
                {
                    ide = idx;
                    break;
                }
                idx = ide;
            }

            sb.Append(str.Substring(lastPut));

            // replacing escape sequences
            sb.Replace(escapeChar + markupStart, markupStart);

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">Source string</param>
        /// <param name="from">Index in the string where first occurence of markupStart CAN be</param>
        /// <returns>Index of text after markup substring or -1 if not found</returns>
        public int FindVariablePlaceholder(string s, int from)
        {
            int i = from;
            int n;

            while (i >= 0 && i < s.Length)
            {
                n = s.IndexOf(markupStart, i);
                if (n < 0)
                {
                    return -1;
                }
                else if (n >= escapeChar.Length)
                {
                    if (s.Substring(n - escapeChar.Length, escapeChar.Length).Equals(escapeChar))
                        i = n + markupStart.Length;
                    else
                        return n + markupStart.Length;
                }
                else
                {
                    return n + markupStart.Length;
                }
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">source string</param>
        /// <param name="from">index in string where next occurence of markupEnd can be</param>
        /// <returns>Index of next markupEnd substring</returns>
        public int FindVariablePlaceholderEnd(string s, int from)
        {
            int i = s.IndexOf(markupEnd, from);
            if (i > 0)
                return i;
            else
                return -1;
        }

        /// <summary>
        /// Formating string:
        /// 20s   - string with padding to 20 chars, left align
        /// -20s  - string with padding to 20 chars, right align
        /// 2d    - integer number padded to 2 chars with spaces, right align
        /// 02d   - integer number padded to 2 chars with zero, right align
        /// 1.7f  - floating point value with at least 1 digit before point and 7 digits after point
        /// </summary>
        /// <param name="ph">Input placeholder (without markup substrings,
        /// this can contain also some formatting after : character</param>
        /// <returns></returns>
        public string ReplaceVariablePlaceholder(string ph)
        {
            string fmt = "";
            int phi = ph.IndexOf(':');
            if (phi >= 0)
            {
                fmt = ph.Substring(phi + 1);
                ph = ph.Substring(0, phi);
            }

            GSCore cs = EvaluateProperty(ph);
            if (fmt.EndsWith("s"))
            {
                string value = cs.getStringValue();
                int places;
                if (int.TryParse(fmt.Substring(0,fmt.Length - 1), out places))
                {
                    if (places > 0)
                        value = value.PadRight(places);
                    else
                        value = value.PadLeft(-places);
                }
                return value;
            }
            else if (fmt.EndsWith("m"))
            {
                string value = cs.getStringValue();
                int places;
                if (int.TryParse(fmt.Substring(0, fmt.Length - 1), out places))
                {
                    if (value.Length > places)
                    {
                        value = value.Substring(0, places - 3) + "...";
                    }
                    else
                    {
                        if (places > 0)
                            value = value.PadRight(places);
                        else
                            value = value.PadLeft(-places);
                    }
                }
                return value;
            }
            else if (fmt.EndsWith("d"))
            {
                bool padWithZero = false;
                int places;
                string result = "";
                long ival = cs.getIntegerValue();
                if (int.TryParse(fmt.Substring(0, fmt.Length - 1), out places))
                {
                    if (fmt.StartsWith("0"))
                        padWithZero = true;
                    if (padWithZero)
                    {
                        result = string.Format("{0:0".PadRight(places - 1, '0') + "}", ival);
                        result = result.PadLeft(places, '0');
                    }
                    else
                    {
                        result = string.Format("{0:#".PadRight(places - 1, '#') + "}", ival);
                        result = result.PadLeft(places, ' ');
                    }

                }
                else
                {
                    result = ival.ToString();
                }
                return result;
            }
            else if (fmt.EndsWith("f"))
            {
                string a, b;
                fmt = fmt.Substring(0, fmt.Length - 1);
                int i = fmt.IndexOf('.');
                if (i >= 0)
                {
                    a = fmt.Substring(0, i);
                    b = fmt.Substring(i + 1);
                }
                else
                {
                    a = fmt;
                    b = "0";
                }
                int ia, ib;
                double d = cs.getDoubleValue();
                string result;
                if (int.TryParse(a, out ia) && int.TryParse(b, out ib))
                {
                    result = string.Format("{0:" + string.Format("F{0}", ib) + "}", d);
                    result = result.PadLeft(ia + ib + 1);
                }
                else
                {
                    result = d.ToString();
                }
                return result;
            }
            else
            {
                return cs.getStringValue();
            }
        }

        #endregion

        private GSCore execDo(GSCoreCollection args)
        {
            GSCore last = null;
            foreach (GSCore item in args)
            {
                last = ExecuteElement(item);
                if (last is GSReturn)
                {
                    return last;
                }

            }
            return last;
        }

        private GSCore execMessage(GSCoreCollection args)
        {
            GSCore result = GSCore.Void;

            // first is token, name of variable, object
            // second is token, message name
            // third etc are arguments
            if (args.Count >= 2 && args.getSafe(0) is GSToken && args.getSafe(1) is GSToken)
            {
                // evaluate the remaining portion of list
                GSCoreCollection subArgs = getNativeValues(args.getSublist(2));
                // first and second tokens
                GSToken t1 = args.getSafe(0) as GSToken;
                GSToken t2 = args.getSafe(1) as GSToken;
                // evaluate reference to object
                GSCore obj = ExecuteElement(t1);
                // execute message in the object
                result = obj.ExecuteMessage(t2.Token, subArgs);
            }

            return result;
        }

        private GSCore execForeach(GSCoreCollection args)
        {
            if (args.Count < 4)
            {
                Debugger.Log(0, "", "Insufficient arguments for (FOREACH varName : list commands ) ");
                return null;
            }
            GSCore t1 = args.getSafe(0);
            GSCore l1 = ExecuteElement(args.getSafe(2));
            if (!(t1 is GSToken))
            {
                Debugger.Log(0, "", "Token shoudl be second argument in FOREACH ");
                return null;
            }
            if (!(l1 is GSList))
            {
                Debugger.Log(0, "", "List should be fourth argument in FOREACH ");
                return null;
            }
            GSToken tk = (GSToken)t1;
            GSList lst = (GSList)l1;
            GSCore r = null;
            int ik = 0;

            foreach (GSCore item in lst.Parts)
            {
                SetVariable(tk.Token, item);
                for (int i = 3; i < args.Count; i++)
                {
                    r = ExecuteElement(args.getSafe(i));
                    if (r is GSReturn)
                    {
                        break;
                    }
                }
                ik++;

                if (r is GSReturn)
                {
                    GSReturn ret = r as GSReturn;
                    if (ret.Type == GSReturn.TYPE_BREAK)
                        break;
                    if (ret.Type == GSReturn.TYPE_RETURN)
                        return ret;
                }
            }

            return new GSNumber(ik);
        }

        private GSCore execWhile(GSCoreCollection args)
        {
            GSCoreCollection commands = args.getSublist(1);
            GSCore r = null;

            while (ExecuteElement(args.getSafe(0)).getBooleanValue())
            {
                foreach (GSCore cmd in commands)
                {
                    r = ExecuteElement(cmd);
                    if (r is GSReturn)
                    {
                        break;
                    }
                }

                if (r is GSReturn)
                {
                    GSReturn ret = r as GSReturn;
                    if (ret.Type == GSReturn.TYPE_BREAK)
                        break;
                    if (ret.Type == GSReturn.TYPE_RETURN)
                        return ret;
                }
            }

            return GSCore.Void;
        }

        private GSCore execIf(GSCoreCollection args)
        {
            GSCore cond = ExecuteElement(args.getSafe(0));
            GSCore cmd1 = args.getSafe(1);
            GSCore cmd2 = args.getSafe(2);
            GSCore r = null;

            if (cond.getBooleanValue())
            {
                bool running = false;
                foreach (GSCore cmd in args)
                {
                    if (cmd is GSToken && cmd.ToString().Equals("then"))
                        running = true;
                    if (cmd is GSToken && cmd.ToString().Equals("else"))
                        running = false;
                    if (running)
                    {
                        r = ExecuteElement(cmd);
                        if (r is GSReturn)
                            return r;
                    }
                }
            }
            else
            {
                bool running = false;
                foreach (GSCore cmd in args)
                {
                    if (cmd is GSToken && cmd.ToString().Equals("else"))
                        running = true;
                    if (running)
                    {
                        r = ExecuteElement(cmd);
                        if (r is GSReturn)
                            return r;
                    }
                }
            }

            return cond;
        }

        private GSCore execPrint(GSCoreCollection arg, bool newLine)
        {
            foreach (GSCore argument in arg)
            {
                GSCore val = ExecuteElement(argument);
                string str = val.getStringValue();
                str = ReplaceVariables(str);
                output.Append(str);
            }

            if (newLine)
                output.AppendLine();

            return GSCore.Void;
        }

        private GSCore execGt(GSCoreCollection arg1)
        {
            GSBoolean bv = new GSBoolean();
            GSCoreDataType dt = arg1.getArithmeticDataType();

            if (dt == GSCoreDataType.Double)
            {
                bv.Value = (arg1[0].getDoubleValue() > arg1[1].getDoubleValue());
            }
            else if (dt == GSCoreDataType.Integer)
            {
                bv.Value = (arg1[0].getIntegerValue() > arg1[1].getIntegerValue());
            }
            else if (dt == GSCoreDataType.String)
            {
                bv.Value = (arg1[0].getStringValue().CompareTo(arg1[1].getStringValue()) > 0);
            }

            return bv;
        }

        private GSCore execGe(GSCoreCollection arg1)
        {
            GSBoolean bv = new GSBoolean();
            GSCoreDataType dt = arg1.getArithmeticDataType();

            if (dt == GSCoreDataType.Double)
            {
                bv.Value = (arg1[0].getDoubleValue() >= arg1[1].getDoubleValue());
            }
            else if (dt == GSCoreDataType.Integer)
            {
                bv.Value = (arg1[0].getIntegerValue() >= arg1[1].getIntegerValue());
            }
            else if (dt == GSCoreDataType.String)
            {
                bv.Value = (arg1[0].getStringValue().CompareTo(arg1[1].getStringValue()) >= 0);
            }

            return bv;
        }

        private GSCore execEq(GSCoreCollection arg1)
        {
            GSBoolean bv = new GSBoolean();
            GSCoreDataType dt = arg1.getArithmeticDataType();

            if (dt == GSCoreDataType.Double)
            {
                bv.Value = (arg1[0].getDoubleValue() == arg1[1].getDoubleValue());
            }
            else if (dt == GSCoreDataType.Integer)
            {
                bv.Value = (arg1[0].getIntegerValue() == arg1[1].getIntegerValue());
            }
            else if (dt == GSCoreDataType.String)
            {
                bv.Value = (arg1[0].getStringValue().CompareTo(arg1[1].getStringValue()) == 0);
            }

            return bv;
        }

        private GSCore execNe(GSCoreCollection arg1)
        {
            GSBoolean bv = new GSBoolean();
            GSCoreDataType dt = arg1.getArithmeticDataType();

            if (dt == GSCoreDataType.Double)
            {
                bv.Value = (arg1[0].getDoubleValue() != arg1[1].getDoubleValue());
            }
            else if (dt == GSCoreDataType.Integer)
            {
                bv.Value = (arg1[0].getIntegerValue() != arg1[1].getIntegerValue());
            }
            else if (dt == GSCoreDataType.String)
            {
                bv.Value = (arg1[0].getStringValue().CompareTo(arg1[1].getStringValue()) != 0);
            }

            return bv;
        }

        private GSCore execLe(GSCoreCollection arg1)
        {
            GSBoolean bv = new GSBoolean();
            GSCoreDataType dt = arg1.getArithmeticDataType();

            if (dt == GSCoreDataType.Double)
            {
                bv.Value = (arg1[0].getDoubleValue() <= arg1[1].getDoubleValue());
            }
            else if (dt == GSCoreDataType.Integer)
            {
                bv.Value = (arg1[0].getIntegerValue() <= arg1[1].getIntegerValue());
            }
            else if (dt == GSCoreDataType.String)
            {
                bv.Value = (arg1[0].getStringValue().CompareTo(arg1[1].getStringValue()) <= 0);
            }

            return bv;
        }

        private GSCore execLt(GSCoreCollection arg1)
        {
            GSBoolean bv = new GSBoolean();
            GSCoreDataType dt = arg1.getArithmeticDataType();

            if (dt == GSCoreDataType.Double)
            {
                bv.Value = (arg1[0].getDoubleValue() < arg1[1].getDoubleValue());
            }
            else if (dt == GSCoreDataType.Integer)
            {
                bv.Value = (arg1[0].getIntegerValue() < arg1[1].getIntegerValue());
            }
            else if (dt == GSCoreDataType.String)
            {
                bv.Value = (arg1[0].getStringValue().CompareTo(arg1[1].getStringValue()) < 0);
            }

            return bv;
        }

        private GSCore execSet(GSCore keyElem, GSCore valueElem)
        {
            string key;
            if (keyElem is GSToken)
                key = (keyElem as GSToken).Token;
            else
                key = keyElem.getStringValue();
            GSCore value = ExecuteElement(valueElem);
            SetVariable(key, value);
            SetSystemVariables(key, value);
            return value;
        }

        private void SetSystemVariables(string key, GSCore value)
        {
            if (key.Equals("escapeChar"))
                escapeChar = value.getStringValue();
            else if (key.Equals("markupStart"))
                markupStart = value.getStringValue();
            else if (key.Equals("markupEnd"))
                markupEnd = value.getStringValue();
        }

        private GSCore execNot(GSCoreCollection args)
        {
            bool result = true;

            if (args.Count > 0)
                result = !args[0].getBooleanValue();
            return new GSBoolean() { Value = result };
        }

        private GSCore execOr(GSCoreCollection args)
        {
            bool result = false;
            foreach (GSCore item in args)
            {
                if (item.getBooleanValue() == true)
                {
                    result = true;
                    break;
                }
            }
            return new GSBoolean() { Value = result };
        }

        private GSCore execAnd(GSCoreCollection args)
        {
            bool result = true;
            foreach (GSCore item in args)
            {
                if (item.getBooleanValue() == false)
                {
                    result = false;
                    break;
                }
            }
            return new GSBoolean() { Value = result };
        }

        private GSCore execDiv(GSCoreCollection args)
        {
            GSCoreDataType dataType = args.getArithmeticDataType();

            switch (dataType)
            {
                case GSCoreDataType.Double:
                    {
                        double[] arr = getDoubleArray(args);
                        double sum = arr[0];
                        for (int i = 1; i < arr.Length; i++)
                            sum /= arr[i];
                        return new GSNumber() { DoubleValue = sum };
                    }
                case GSCoreDataType.Integer:
                case GSCoreDataType.Boolean:
                    {
                        long[] arr = getIntegerArray(args);
                        long sum = arr[0];
                        for (int i = 1; i < arr.Length; i++)
                            sum /= arr[i];
                        return new GSNumber() { IntegerValue = sum };
                    }
                default:
                    break;
            }

            return new GSString();
        }

        private GSCore execMul(GSCoreCollection args)
        {
            GSCoreDataType dataType = args.getArithmeticDataType();

            switch (dataType)
            {
                case GSCoreDataType.Double:
                    {
                        double[] arr = getDoubleArray(args);
                        double sum = 1.0;
                        for (int i = 0; i < arr.Length; i++)
                            sum *= arr[i];
                        return new GSNumber() { DoubleValue = sum };
                    }
                case GSCoreDataType.Integer:
                case GSCoreDataType.Boolean:
                    {
                        long[] arr = getIntegerArray(args);
                        long sum = 1;
                        for (int i = 0; i < arr.Length; i++)
                            sum *= arr[i];
                        return new GSNumber() { IntegerValue = sum };
                    }
                default:
                    break;
            }

            return new GSString();
        }

        private GSCore execSub(GSCoreCollection args)
        {
            GSCoreDataType dataType = args.getArithmeticDataType();

            switch (dataType)
            {
                case GSCoreDataType.Double:
                    {
                        double[] arr = getDoubleArray(args);
                        double sum = arr[0];
                        for (int i = 1; i < arr.Length; i++)
                            sum -= arr[i];
                        return new GSNumber() { DoubleValue = sum };
                    }
                case GSCoreDataType.Integer:
                case GSCoreDataType.Boolean:
                    {
                        long[] arr = getIntegerArray(args);
                        long sum = arr[0];
                        for (int i = 1; i < arr.Length; i++)
                            sum -= arr[i];
                        return new GSNumber() { IntegerValue = sum };
                    }
                default:
                    break;
            }

            return new GSString();
        }

        private GSCore execAdd(GSCoreCollection args)
        {
            GSCoreDataType dataType = args.getArithmeticDataType();

            switch (dataType)
            {
                case GSCoreDataType.String:
                case GSCoreDataType.Void:
                    {
                        string[] arr = getStringArray(args);
                        StringBuilder sb = new StringBuilder();
                        foreach (string s in arr)
                        {
                            if (sb.Length > 0)
                                sb.Append(' ');
                            sb.Append(s);
                        }
                        return new GSString() { Value = sb.ToString() };
                    }
                case GSCoreDataType.Double:
                    {
                        double[] arr = getDoubleArray(args);
                        double sum = 0;
                        for (int i = 0; i < arr.Length; i++)
                            sum += arr[i];
                        return new GSNumber() { DoubleValue = sum };
                    }
                case GSCoreDataType.Integer:
                case GSCoreDataType.Boolean:
                    {
                        long[] arr = getIntegerArray(args);
                        long sum = 0;
                        for (int i = 0; i < arr.Length; i++)
                            sum += arr[i];
                        return new GSNumber() { IntegerValue = sum };
                    }
                default:
                    break;
            }

            return new GSString();
        }


        // this function is also in GSCore object
        // evaluates property into value
        public override GSCore GetPropertyValue(string Token)
        {
            // find in variables
            GSCore obj = GetVariable(Token);
            if (obj != null)
                return obj;

            // find built-in property
            if (Token.Equals("name"))
                return new GSString() { Value = "Executor" };

            // return default empty string
            return new GSString();
        }


        /// <summary>
        /// Looks for object with given name in the stack of variables
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GSCore GetVariable(string name)
        {
            for (int i = stackVars.Count - 1; i >= 0; i--)
            {
                if (stackVars[i].ContainsKey(name))
                {
                    return stackVars[i][name];
                }
            }

            return null;
        }

        /// <summary>
        /// Sets value for variable.
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varValue"></param>
        public void SetVariable(string varName, GSCore varValue)
        {
            Dictionary<string, GSCore> vars = getLastVars();
            if (vars.ContainsKey(varName))
            {
                vars[varName] = varValue;
            }
            else
            {
                vars.Add(varName, varValue);
            }
        }

        /// <summary>
        /// Returning back to previous variable context
        /// </summary>
        public void PopStack()
        {
            if (stackVars.Count > 1)
                stackVars.RemoveAt(stackVars.Count - 1);
        }

        /// <summary>
        /// Creating new variable context
        /// </summary>
        public void PushStack()
        {
            stackVars.Add(new Dictionary<string, GSCore>());
        }

        /// <summary>
        /// Returns output text as a result from script execution.
        /// </summary>
        /// <returns></returns>
        public string getOutput()
        {
            return output.ToString();
        }

        private static int level = 0;
        /// <summary>
        /// Executing element. For most of the elements in the program it is element itself,
        /// but for the list it is result of executing operation that is mentioned in the head
        /// of the list.
        /// </summary>
        /// <param name="E"></param>
        /// <returns></returns>
        public GSCore ExecuteElement(GSCore E)
        {
            level++;
            GSCore result =  ExecuteElementWithoutreport(E);
            /*Debugger.Log(0, "", "".PadLeft(level) + "Script: " + E.ToString() + "\r\n");
            Debugger.Log(0, "", "".PadLeft(level) + "Result: " + result.ToString() + "\r\n");
            if (level == 1)
            {
                foreach(Dictionary<string,GSCore> vd in stackVars)
                {
                    foreach(KeyValuePair<string,GSCore> v in vd)
                    {
                        Debugger.Log(0, "", "".PadLeft(level) + "Var: [" + v.Key + "] = " + v.Value.ToString() + "\r\n");
                    }
                }
            }*/
            level--;

            return result;
        }

        private GSCore ExecuteElementWithoutreport(GSCore E)
        {
            if (E is GSList)
            {
                GSList L = (GSList)E;
                if (L.Count == 0)
                {
                    return GSCore.Void;
                }
                else if (L.Count == 1)
                {
                    return ExecuteElement(L[0]);
                }
                else
                {
                    if (L.Parts.IsFirstToken())
                    {
                        GSCore res = null;
                        try
                        {
                            res = ExecuteMessage(L.Parts.getFirstToken(),
                                L.Parts.getSublist(1));
                        }
                        catch
                        {
                            res = new GSString();
                        }
                        finally
                        {
                        }
                        return res;
                    }
                    else
                    {
                        GSCore result = null;
                        foreach (GSCore item in L.Parts)
                        {
                            result = ExecuteElement(item);
                            if (result is GSReturn)
                                break;
                        }
                        if (result == null)
                            return new GSString();
                        return result;
                    }
                }
            }
            else if (E is GSToken)
            {
                return EvaluateProperty(((GSToken)E).Token);
            }
            else
            {
                return E;
            }
        }

        public void resetOutput()
        {
            output.Clear();
        }
    }
}
