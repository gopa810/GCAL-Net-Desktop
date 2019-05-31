GCFestivalBook:Appearance Days of the Lord and His Incarnations|0|1
GCFestivalTithiMasa:Lord Nrsimhadeva|Nrsimha Caturdasi: Appearance of Lord Nrsimhadeva|0|7|1|-10000|1|0|0|28|0
GCFestivalTithiMasa:Lord Balarama|Lord Balarama -- Appearance|0|7|1|-10000|1|0|0|29|3
GCFestivalTithiMasa:Srimati Radharani|Radhastami: Appearance of Srimati Radharani|0|0|1|-10000|1|0|0|22|4
GCFestivalTithiMasa:Vamanadeva|Sri Vamana Dvadasi: Appearance of Lord Vamanadeva|0|7|1|-10000|1|0|0|26|4
GCFestivalTithiMasa:Advaita Acarya|Sri Advaita Acarya -- Appearance|0|7|1|-10000|1|0|0|21|9
GCFestivalTithiMasa:Varahadeva|Varaha Dvadasi: Appearance of Lord Varahadeva|0|7|1|-10000|1|0|0|26|9
GCFestivalTithiMasa:Sri Nityananda|Nityananda Trayodasi: Appearance of Sri Nityananda Prabhu|0|7|1|-10000|1|0|0|27|9
GCFestivalSpecial:Sri Krsna|Sri Krsna Janmastami: Appearance of Lord Sri Krsna|0|7|1|-10000|1|10|0|4|4
GCFestivalRelated:|Nandotsava|1|0|1|-10000|1|13|1
GCFestivalRelated:Srila Prabhupada|Srila Prabhupada -- Appearance|2|0|1|-10000|1|15|1
BeginScript
(set a0 0)
(set a1 0)
(set a2 0)
(set d0 (day -1))
(set d1 (day 0))
(set d2 (day 1))
(if (== d0.ksayaTithi 7) then (return 1))
(if (== d0.astro.tithi 7) then (set a0 16))
(if (== d1.astro.tithi 7) then (set a1 16))
(if (== d2.astro.tithi 7) then (set a2 16))
(if (== a1 0) then (return 0))
(if (and (< a0 a1) (> a1 a2)) then (return 1))
(if (== d0.astro.naksatra 3) then (set a0 (+ a0 8)))
(if (== d1.astro.naksatra 3) then (set a1 (+ a1 8)))
(if (== d2.astro.naksatra 3) then (set a2 (+ a2 8)))
(if (and (< a0 a1) (> a1 a2)) then (return 1))
(if (or  (> a0 a1) (> a2 a1)) then (return 0))
(if (== d0.midnightNaksatra 3) then (set a0 (+ a0 4)))
(if (== d1.midnightNaksatra 3) then (set a1 (+ a1 4)))
(if (== d2.midnightNaksatra 3) then (set a2 (+ a2 4)))
(if (and (< a0 a1) (> a1 a2)) then (return 1))
(if (or  (> a0 a1) (> a2 a1)) then (return 0))
(if (== d0.date.weekday 0) then (set a0 (+ a0 2)))
(if (== d1.date.weekday 0) then (set a1 (+ a1 2)))
(if (== d2.date.weekday 0) then (set a2 (+ a2 2)))
(if (== d0.date.weekday 2) then (set a0 (+ a0 2)))
(if (== d1.date.weekday 2) then (set a1 (+ a1 2)))
(if (== d2.date.weekday 2) then (set a2 (+ a2 2)))
(if (== d2.astro.tithi 7) then (set a1 (+ a1 1)))
(if (and (< a0 a1) (> a1 a2)) then (return 1))
(return 0)
EndScript
GCFestivalTithiMasa:Sri Krsna Caitanya Mahaprabhu|Gaura Purnima: Appearance of Sri Caitanya Mahaprabhu|0|7|1|-10000|1|12|0|29|10
GCFestivalSpecial:Sri Ramacandra|Rama Navami: Appearance of Lord Sri Ramacandra|0|7|1|-10000|1|9|0|11|11
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(set d2 (day 1))
(set d3 (day 2))
(if (== d0.ksayaTithi 23) then (return 1))
(if (== d0.astro.tithi 23) then (return 0))
(if (and (== d2.astro.tithi 23) (> d3.fastType 0)) then (return 1))
(if (and (== d1.astro.tithi 23) (== d2.fastType 0)) then (return 1))
(return 0)
EndScript
GCFestivalBook:Events in the Pastimes of the Lord and His Associates|1|1
GCFestivalTithiMasa:|Aksaya Trtiya. Candana Yatra starts. (Continues for 21 days)|1|0|1|-10000|1|0|0|17|0
GCFestivalTithiMasa:|Srimati Sita Devi (consort of Lord Sri Rama) -- Appearance|1|0|1|-10000|1|0|0|23|0
GCFestivalTithiMasa:|Rukmini Dvadasi|1|0|1|-10000|1|0|0|26|0
GCFestivalTithiMasa:|Krsna Phula Dola, Salila Vihara|1|0|1|-10000|1|0|0|29|0
GCFestivalTithiMasa:|Sri Sri Radha-Ramana Devaji -- Appearance|1|0|1|-10000|1|0|0|29|0
GCFestivalTithiMasa:|Ganga Puja|1|0|1|-10000|1|0|0|24|1
GCFestivalTithiMasa:|Panihati Cida Dahi Utsava|1|0|1|-10000|1|0|0|27|1
GCFestivalTithiMasa:|Snana Yatra|1|0|1|-10000|1|0|0|29|1
GCFestivalTithiMasa:|Guru (Vyasa) Purnima|1|0|1|-10000|1|0|0|29|2
GCFestivalTithiMasa:|Radha Govinda Jhulana Yatra begins|1|0|1|-10000|1|0|0|25|3
GCFestivalTithiMasa:|Jhulana Yatra ends|1|0|1|-10000|1|0|0|29|3
GCFestivalTithiMasa:|Ananta Caturdasi Vrata|1|0|1|-10000|1|0|0|28|4
GCFestivalTithiMasa:|Sri Visvarupa Mahotsava|1|0|1|-10000|1|0|0|29|4
GCFestivalTithiMasa:|Ramacandra Vijayotsava|1|0|1|-10000|1|0|0|24|5
GCFestivalTithiMasa:|Sri Krsna Saradiya Rasayatra|1|0|1|-10000|1|0|0|29|5
GCFestivalTithiMasa:|Appearance of Radha Kunda, snana dana|1|0|1|-10000|1|0|0|7|6
GCFestivalTithiMasa:|Bahulastami|1|0|1|-10000|1|0|0|7|6
GCFestivalTithiMasa:|Dipa dana, Dipavali, (Kali Puja)|1|0|1|-10000|1|0|0|14|6
GCFestivalTithiMasa:|Bali Daityaraja Puja|1|0|1|-10000|1|0|0|15|6
GCFestivalTithiMasa:|Gopastami, Gosthastami|1|0|1|-10000|1|0|0|22|6
GCFestivalTithiMasa:|Sri Krsna Rasayatra|1|0|1|-10000|1|0|0|29|6
GCFestivalTithiMasa:|Tulasi-Saligrama Vivaha (marriage)|1|0|1|-10000|1|0|0|29|6
GCFestivalTithiMasa:|Katyayani vrata begins|1|0|1|-10000|1|0|0|0|7
GCFestivalTithiMasa:|Odana sasthi|1|0|1|-10000|1|0|0|20|7
GCFestivalTithiMasa:|Advent of Srimad Bhagavad-gita|1|0|1|-10000|1|0|0|25|7
GCFestivalTithiMasa:|Katyayani vrata ends|1|0|1|-10000|1|0|0|29|7
GCFestivalTithiMasa:|Sri Krsna Pusya Abhiseka|1|0|1|-10000|1|0|0|29|8
GCFestivalTithiMasa:|Vasanta Pancami|1|0|1|-10000|1|0|0|19|9
GCFestivalTithiMasa:|Bhismastami|1|0|1|-10000|1|0|0|22|9
GCFestivalTithiMasa:|Sri Krsna Madhura Utsava|1|0|1|-10000|1|0|0|29|9
GCFestivalTithiMasa:|Siva Ratri|1|0|1|-10000|1|0|0|13|10
GCFestivalTithiMasa:|Damanakaropana Dvadasi|1|0|1|-10000|1|0|0|26|11
GCFestivalTithiMasa:|Sri Balarama Rasayatra|1|0|1|-10000|1|0|0|29|11
GCFestivalTithiMasa:|Sri Krsna Vasanta Rasa|1|0|1|-10000|1|0|0|29|11
GCFestivalSpecial:|Go Puja. Go Krda. Govardhana Puja.|1|0|1|-10000|1|6|0|6|6
BeginScript
(set a0 0)
(set a1 0)
(set a2 0)
(set d0 (day -1))
(set d1 (day 0))
(set d2 (day 1))
(if (!= d1.astro.masa 6) then (return 0))
(if (== d0.ksayaTithi 15) then (return 1))
(if (== d0.astro.tithi 15) then (set a0 16))
(if (== d1.astro.tithi 15) then (set a1 16))
(if (== d2.astro.tithi 15) then (set a2 16))
(if (== a1 0) then (return 0))
(if (and (< a0 a1) (> a1 a2)) then (return 1))
(if (< d0.moonRiseSec d0.sunRiseSec) then (set a0 (+ a0 8)))
(if (< d1.moonRiseSec d1.sunRiseSec) then (set a1 (+ a1 8)))
(if (< d2.moonRiseSec d2.sunRiseSec) then (set a2 (+ a2 8)))
(if (or (> a0 a1) (> a2 a1)) then (return 0))
(return 1)
EndScript
GCFestivalTithiMasa:|Ratha Yatra|1|0|1|-10000|1|11|0|16|2
GCFestivalTithiMasa:|Gundica Marjana|1|0|1|-10000|1|5|-1|16|2
GCFestivalTithiMasa:|Return Ratha (8 days after Ratha Yatra)|1|0|1|-10000|1|3|8|16|2
GCFestivalTithiMasa:|Hera Pancami (4 days after Ratha Yatra)|1|0|1|-10000|1|4|4|16|2
GCFestivalTithiMasa:|Festival of Jagannatha Misra|1|0|1|-10000|1|14|1|29|10
GCFestivalEkadasi:|First day of Bhisma Pancaka|1|0|1|-10000|1|0|0|6|1
GCFestivalMasaDay:|Last day of Bhisma Pancaka|1|0|1|-10000|1|0|0|29|6
GCFestivalBook:Appearance and Disappearance Days of Recent Acaryas|2|1
GCFestivalTithiMasa:Bhaktivinoda Thakura|Srila Bhaktivinoda Thakura -- Disappearance|2|0|1|-10000|1|0|0|14|2
GCFestivalTithiMasa:|Sri Vamsidasa Babaji -- Disappearance|2|0|1|-10000|1|0|0|18|3
GCFestivalTithiMasa:Bhaktivinoda Thakura|Srila Bhaktivinoda Thakura -- Appearance|2|0|1|-10000|1|0|0|27|4
GCFestivalTithiMasa:|Sri Madhvacarya -- Appearance|2|0|1|-10000|1|0|0|24|5
GCFestivalTithiMasa:Srila Prabhupada|Srila Prabhupada -- Disappearance|2|0|1|-10000|1|0|0|18|6
GCFestivalTithiMasa:Gaura Kisora Dasa Babaji|Srila Gaura Kisora Dasa Babaji -- Disappearance|2|0|1|-10000|1|0|0|25|6
GCFestivalTithiMasa:Bhaktisiddhanta Sarasvati|Srila Bhaktisiddhanta Sarasvati Thakura -- Disappearance|2|0|1|-10000|1|0|0|3|8
GCFestivalTithiMasa:Bhaktisiddhanta Sarasvati|Srila Bhaktisiddhanta Sarasvati Thakura -- Appearance|2|0|1|-10000|1|0|0|4|10
GCFestivalTithiMasa:|Srila Jagannatha Dasa Babaji -- Disappearance|2|0|1|-10000|1|0|0|15|10
GCFestivalBook:Appearance and Disappearance Days of Mahaprabhu's Associates and Other Acaryas|3|1
GCFestivalTithiMasa:|Sri Abhirama Thakura -- Disappearance|3|0|1|-10000|1|0|0|6|0
GCFestivalTithiMasa:|Srila Vrndavana Dasa Thakura -- Disappearance|3|0|1|-10000|1|0|0|9|0
GCFestivalTithiMasa:|Sri Gadadhara Pandita -- Appearance|3|0|1|-10000|1|0|0|14|0
GCFestivalTithiMasa:|Sri Madhu Pandita -- Disappearance|3|0|1|-10000|1|0|0|23|0
GCFestivalTithiMasa:|Srimati Jahnava Devi -- Appearance|3|0|1|-10000|1|0|0|23|0
GCFestivalTithiMasa:|Sri Paramesvari Dasa Thakura -- Disappearance|3|0|1|-10000|1|0|0|29|0
GCFestivalTithiMasa:|Sri Madhavendra Puri -- Appearance|3|0|1|-10000|1|0|0|29|0
GCFestivalTithiMasa:|Sri Srinivasa Acarya -- Appearance|3|0|1|-10000|1|0|0|29|0
GCFestivalTithiMasa:|Sri Ramananda Raya -- Disappearance|3|0|1|-10000|1|0|0|4|1
GCFestivalTithiMasa:|Srila Vrndavana Dasa Thakura -- Appearance|3|0|1|-10000|1|0|0|11|1
GCFestivalTithiMasa:|Sri Baladeva Vidyabhusana -- Disappearance|3|0|1|-10000|1|0|0|24|1
GCFestivalTithiMasa:|Srimati Gangamata Gosvamini -- Appearance|3|0|1|-10000|1|0|0|24|1
GCFestivalTithiMasa:|Sri Mukunda Datta -- Disappearance|3|0|1|-10000|1|0|0|29|1
GCFestivalTithiMasa:|Sri Sridhara Pandita -- Disappearance|3|0|1|-10000|1|0|0|29|1
GCFestivalTithiMasa:|Sri Syamananda Prabhu -- Disappearance|3|0|1|-10000|1|0|0|0|2
GCFestivalTithiMasa:|Sri Vakresvara Pandita -- Appearance|3|0|1|-10000|1|0|0|4|2
GCFestivalTithiMasa:|Sri Srivasa Pandita -- Disappearance|3|0|1|-10000|1|0|0|9|2
GCFestivalTithiMasa:|Sri Gadadhara Pandita -- Disappearance|3|0|1|-10000|1|0|0|14|2
GCFestivalTithiMasa:|Sri Svarupa Damodara Gosvami -- Disappearance|3|0|1|-10000|1|0|0|16|2
GCFestivalTithiMasa:|Sri Sivananda Sena -- Disappearance|3|0|1|-10000|1|0|0|16|2
GCFestivalTithiMasa:|Sri Vakresvara Pandita -- Disappearance|3|0|1|-10000|1|0|0|20|2
GCFestivalTithiMasa:|Srila Sanatana Gosvami -- Disappearance|3|0|1|-10000|1|0|0|29|2
GCFestivalTithiMasa:|Srila Gopala Bhatta Gosvami -- Disappearance|3|0|1|-10000|1|0|0|4|3
GCFestivalTithiMasa:|Srila Lokanatha Gosvami -- Disappearance|3|0|1|-10000|1|0|0|7|3
GCFestivalTithiMasa:|Sri Raghunandana Thakura -- Disappearance|3|0|1|-10000|1|0|0|18|3
GCFestivalTithiMasa:|Srila Rupa Gosvami -- Disappearance|3|0|1|-10000|1|0|0|26|3
GCFestivalTithiMasa:|Sri Gauridasa Pandita -- Disappearance|3|0|1|-10000|1|0|0|26|3
GCFestivalTithiMasa:|Srimati Sita Thakurani (Sri Advaita's consort) -- Appearance|3|0|1|-10000|1|0|0|19|4
GCFestivalTithiMasa:|Srila Jiva Gosvami -- Appearance|3|0|1|-10000|1|0|0|26|4
GCFestivalTithiMasa:|Srila Haridasa Thakura -- Disappearance|3|0|1|-10000|1|0|0|28|4
GCFestivalTithiMasa:|Srila Raghunatha Dasa Gosvami -- Disappearance|3|0|1|-10000|1|0|0|26|5
GCFestivalTithiMasa:|Srila Raghunatha Bhatta Gosvami -- Disappearance|3|0|1|-10000|1|0|0|26|5
GCFestivalTithiMasa:|Srila Krsnadasa Kaviraja Gosvami -- Disappearance|3|0|1|-10000|1|0|0|26|5
GCFestivalTithiMasa:|Sri Murari Gupta -- Disappearance|3|0|1|-10000|1|0|0|29|5
GCFestivalTithiMasa:|Srila Narottama Dasa Thakura -- Disappearance|3|0|1|-10000|1|0|0|4|6
GCFestivalTithiMasa:|Sri Virabhadra -- Appearance|3|0|1|-10000|1|0|0|8|6
GCFestivalTithiMasa:|Sri Rasikananda -- Appearance|3|0|1|-10000|1|0|0|15|6
GCFestivalTithiMasa:|Sri Vasudeva Ghosh -- Disappearance|3|0|1|-10000|1|0|0|16|6
GCFestivalTithiMasa:|Sri Gadadhara Dasa Gosvami -- Disappearance|3|0|1|-10000|1|0|0|22|6
GCFestivalTithiMasa:|Sri Dhananjaya Pandita -- Disappearance|3|0|1|-10000|1|0|0|22|6
GCFestivalTithiMasa:|Sri Srinivasa Acarya -- Disappearance|3|0|1|-10000|1|0|0|22|6
GCFestivalTithiMasa:|Sri Bhugarbha Gosvami -- Disappearance|3|0|1|-10000|1|0|0|28|6
GCFestivalTithiMasa:|Sri Kasisvara Pandita -- Disappearance|3|0|1|-10000|1|0|0|28|6
GCFestivalTithiMasa:|Sri Nimbarkacarya -- Appearance|3|0|1|-10000|1|0|0|29|6
GCFestivalTithiMasa:|Sri Narahari Sarakara Thakura -- Disappearance|3|0|1|-10000|1|0|0|10|7
GCFestivalTithiMasa:|Sri Kaliya Krsnadasa -- Disappearance|3|0|1|-10000|1|0|0|11|7
GCFestivalTithiMasa:|Sri Saranga Thakura -- Disappearance|3|0|1|-10000|1|0|0|12|7
GCFestivalTithiMasa:|Sri Devananda Pandita -- Disappearance|3|0|1|-10000|1|0|0|10|8
GCFestivalTithiMasa:|Sri Mahesa Pandita -- Disappearance|3|0|1|-10000|1|0|0|12|8
GCFestivalTithiMasa:|Sri Uddharana Datta Thakura -- Disappearance|3|0|1|-10000|1|0|0|12|8
GCFestivalTithiMasa:|Sri Locana Dasa Thakura -- Appearance|3|0|1|-10000|1|0|0|15|8
GCFestivalTithiMasa:|Srila Jiva Gosvami -- Disappearance|3|0|1|-10000|1|0|0|17|8
GCFestivalTithiMasa:|Sri Jagadisa Pandita -- Disappearance|3|0|1|-10000|1|0|0|17|8
GCFestivalTithiMasa:|Sri Jagadisa Pandita -- Appearance|3|0|1|-10000|1|0|0|26|8
GCFestivalTithiMasa:|Sri Ramacandra Kaviraja -- Disappearance|3|0|1|-10000|1|0|0|4|9
GCFestivalTithiMasa:|Srila Gopala Bhatta Gosvami -- Appearance|3|0|1|-10000|1|0|0|4|9
GCFestivalTithiMasa:|Sri Jayadeva Gosvami -- Disappearance|3|0|1|-10000|1|0|0|5|9
GCFestivalTithiMasa:|Sri Locana Dasa Thakura -- Disappearance|3|0|1|-10000|1|0|0|6|9
GCFestivalTithiMasa:|Srimati Visnupriya Devi -- Appearance|3|0|1|-10000|1|0|0|19|9
GCFestivalTithiMasa:|Srila Visvanatha Cakravarti Thakura -- Disappearance|3|0|1|-10000|1|0|0|19|9
GCFestivalTithiMasa:|Sri Pundarika Vidyanidhi -- Appearance|3|0|1|-10000|1|0|0|19|9
GCFestivalTithiMasa:|Sri Raghunandana Thakura -- Appearance|3|0|1|-10000|1|0|0|19|9
GCFestivalTithiMasa:|Srila Raghunatha Dasa Gosvami -- Appearance|3|0|1|-10000|1|0|0|19|9
GCFestivalTithiMasa:|Sri Madhvacarya -- Disappearance|3|0|1|-10000|1|0|0|23|9
GCFestivalTithiMasa:|Sri Ramanujacarya -- Disappearance|3|0|1|-10000|1|0|0|24|9
GCFestivalTithiMasa:|Srila Narottama Dasa Thakura -- Appearance|3|0|1|-10000|1|0|0|29|9
GCFestivalTithiMasa:|Sri Purusottama Das Thakura -- Disappearance|3|0|1|-10000|1|0|0|4|10
GCFestivalTithiMasa:|Sri Isvara Puri -- Disappearance|3|0|1|-10000|1|0|0|11|10
GCFestivalTithiMasa:|Sri Rasikananda -- Disappearance|3|0|1|-10000|1|0|0|15|10
GCFestivalTithiMasa:|Sri Purusottama Dasa Thakura -- Appearance|3|0|1|-10000|1|0|0|18|10
GCFestivalTithiMasa:|Sri Madhavendra Puri -- Disappearance|3|0|1|-10000|1|0|0|26|10
GCFestivalTithiMasa:|Sri Srivasa Pandita -- Appearance|3|0|1|-10000|1|0|0|7|11
GCFestivalTithiMasa:|Sri Govinda Ghosh -- Disappearance|3|0|1|-10000|1|0|0|11|11
GCFestivalTithiMasa:|Sri Ramanujacarya -- Appearance|3|0|1|-10000|1|0|0|19|11
GCFestivalTithiMasa:|Sri Vamsivadana Thakura -- Appearance|3|0|1|-10000|1|0|0|29|11
GCFestivalTithiMasa:|Sri Syamananda Prabhu -- Appearance|3|0|1|-10000|1|0|0|29|11
GCFestivalBook:ISKCON's Historical Events|4|1
GCFestivalTithiMasa:|Sri Jayananda Prabhu -- Disappearance|4|0|1|-10000|1|0|0|27|0
GCFestivalTithiMasa:|The incorporation of ISKCON in New York|4|0|1|-10000|1|0|0|8|3
GCFestivalTithiMasa:|Srila Prabhupada's departure for the USA|4|0|1|-10000|1|0|0|0|4
GCFestivalTithiMasa:|Acceptance of sannyasa by Srila Prabhupada|4|0|1|-10000|1|0|0|29|4
GCFestivalTithiMasa:|Srila Prabhupada's arrival in the USA|4|0|1|-10000|1|0|0|6|5
GCFestivalBook:Bengal-specific Holidays|5|1
GCFestivalTithiMasa:|Jahnu Saptami|5|0|1|-10000|1|0|0|21|0
GCFestivalTithiMasa:|Durga Puja|5|0|1|-10000|1|0|0|21|5
GCFestivalTithiMasa:|Laksmi Puja|5|0|1|-10000|1|0|0|29|5
GCFestivalTithiMasa:|Jagaddhatri Puja|5|0|1|-10000|1|0|0|23|6
GCFestivalTithiMasa:|Sarasvati Puja|5|0|1|-10000|1|0|0|19|9
GCFestivalSankranti:|Ganga Sagara Mela|5|0|1|-10000|1|0|0|9
GCFestivalSankranti:|Tulasi Jala Dan begins|5|0|1|-10000|1|0|0|0
GCFestivalSankranti:|Tulasi Jala Dan ends|5|0|1|-10000|1|0|-1|1
GCFestivalBook:My Personal Events|6|1
GCFestivalBook:Caturmasya (Pratipat System)|7|1
GCFestivalMasaDay:|First month of Caturmasya begins (Pratipat System)|7|0|1|-10000|1|0|0|0|3
GCFestivalMasaDay:|(green leafy vegetable fast for one month)|7|0|1|-10000|1|0|0|0|3
GCFestivalMasaDay:|Last day of the first Caturmasya month|7|0|1|-10000|1|0|-1|0|4
GCFestivalMasaDay:|Second month of Caturmasya begins (Pratipat System)|7|0|1|-10000|1|0|0|0|4
GCFestivalMasaDay:|(yogurt fast for one month)|7|0|1|-10000|1|0|0|0|4
GCFestivalMasaDay:|Last day of the second Caturmasya month|7|0|1|-10000|1|0|-1|0|5
GCFestivalMasaDay:|Third month of Caturmasya begins (Pratipat System)|7|0|1|-10000|1|0|0|0|5
GCFestivalMasaDay:|(milk fast for one month)|7|0|1|-10000|1|0|0|0|5
GCFestivalMasaDay:|Last day of the third Caturmasya month|7|0|1|-10000|1|0|-1|0|6
GCFestivalMasaDay:|Fourth month of Caturmasya begins (Pratipat System)|7|0|1|-10000|1|0|0|0|6
GCFestivalMasaDay:|(urad dal fast for one month)|7|0|1|-10000|1|0|0|0|6
GCFestivalMasaDay:|Last day of the fourth Caturmasya month|7|0|1|-10000|1|0|-1|0|7
GCFestivalSpecial:|First month of Caturmasya continues|7|0|1|-10000|1|6|0|3|3
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 3)) then (return 1))
(return 0)
EndScript
GCFestivalSpecial:|Second month of Caturmasya continues|7|0|1|-10000|1|6|0|4|4
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 4)) then (return 1))
(return 0)
EndScript
GCFestivalSpecial:|Third month of Caturmasya continues|7|0|1|-10000|1|6|0|5|5
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 5)) then (return 1))
(return 0)
EndScript
GCFestivalSpecial:|Fourth month of Caturmasya continues|7|0|1|-10000|1|6|0|6|6
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 6)) then (return 1))
(return 0)
EndScript
GCFestivalBook:Caturmasya (Purnima System)|8|0
GCFestivalMasaDay:|First month of Caturmasya begins (Purnima System)|8|0|1|-10000|1|0|0|29|2
GCFestivalMasaDay:|(green leafy vegetable fast for one month)|8|0|1|-10000|1|0|0|29|2
GCFestivalMasaDay:|Last day of the first Caturmasya month|8|0|1|-10000|1|0|-1|29|3
GCFestivalMasaDay:|Second month of Caturmasya begins (Purnima System)|8|0|1|-10000|1|0|0|29|3
GCFestivalMasaDay:|(yogurt fast for one month)|8|0|1|-10000|1|0|0|29|3
GCFestivalMasaDay:|Last day of the second Caturmasya month|8|0|1|-10000|1|0|-1|29|4
GCFestivalMasaDay:|Third month of Caturmasya begins (Purnima System)|8|0|1|-10000|1|0|0|29|4
GCFestivalMasaDay:|(milk fast for one month)|8|0|1|-10000|1|0|0|29|4
GCFestivalMasaDay:|Last day of the third Caturmasya month|8|0|1|-10000|1|0|-1|29|5
GCFestivalMasaDay:|Fourth month of Caturmasya begins (Purnima System)|8|0|1|-10000|1|0|0|29|5
GCFestivalMasaDay:|(urad dal fast for one month)|8|0|1|-10000|1|0|0|29|5
GCFestivalMasaDay:|Last day of the fourth Caturmasya month|8|0|1|-10000|1|0|-1|29|6
GCFestivalSpecial:|First month of Caturmasya continues|8|0|1|-10000|1|6|0|3|3
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 3)) then (return 1))
(return 0)
EndScript
GCFestivalSpecial:|Second month of Caturmasya continues|8|0|1|-10000|1|6|0|4|4
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 4)) then (return 1))
(return 0)
EndScript
GCFestivalSpecial:|Third month of Caturmasya continues|8|0|1|-10000|1|6|0|5|5
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 5)) then (return 1))
(return 0)
EndScript
GCFestivalSpecial:|Fourth month of Caturmasya continues|8|0|1|-10000|1|6|0|6|6
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 6)) then (return 1))
(return 0)
EndScript
GCFestivalBook:Caturmasya (Ekadasi System)|9|0
GCFestivalEkadasi:|First month of Caturmasya begins (Ekadasi System)|9|0|1|-10000|1|0|0|2|1
GCFestivalEkadasi:|(green leafy vegetable fast for one month)|9|0|1|-10000|1|0|0|2|1
GCFestivalEkadasi:|Last day of the first Caturmasya month|9|0|1|-10000|1|0|-1|3|1
GCFestivalEkadasi:|Second month of Caturmasya begins (Ekadasi System)|9|0|1|-10000|1|0|0|3|1
GCFestivalEkadasi:|(yogurt fast for one month)|9|0|1|-10000|1|0|0|3|1
GCFestivalEkadasi:|Last day of the second Caturmasya month|9|0|1|-10000|1|0|-1|4|1
GCFestivalEkadasi:|Third month of Caturmasya begins (Ekadasi System)|9|0|1|-10000|1|0|0|4|1
GCFestivalEkadasi:|(milk fast for one month)|9|0|1|-10000|1|0|0|4|1
GCFestivalEkadasi:|Last day of the third Caturmasya month|9|0|1|-10000|1|0|-1|5|1
GCFestivalEkadasi:|Fourth month of Caturmasya begins (Ekadasi System)|9|0|1|-10000|1|0|0|5|1
GCFestivalEkadasi:|(urad dal fast for one month)|9|0|1|-10000|1|0|0|5|1
GCFestivalEkadasi:|Last day of the fourth Caturmasya month|9|0|1|-10000|1|0|-1|6|1
GCFestivalSpecial:|First month of Caturmasya continues|9|0|1|-10000|1|6|0|3|3
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 3)) then (return 1))
(return 0)
EndScript
GCFestivalSpecial:|Second month of Caturmasya continues|9|0|1|-10000|1|6|0|4|4
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 4)) then (return 1))
(return 0)
EndScript
GCFestivalSpecial:|Third month of Caturmasya continues|9|0|1|-10000|1|6|0|5|5
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 5)) then (return 1))
(return 0)
EndScript
GCFestivalSpecial:|Fourth month of Caturmasya continues|9|0|1|-10000|1|6|0|6|6
BeginScript
(set d0 (day -1))
(set d1 (day 0))
(if (and (== d0.astro.masa 12) (== d1.astro.masa 6)) then (return 1))
(return 0)
EndScript
