# 🍔 Optimizarea Managementului Comenzilor Fast-Food utilizând NSGA-II

[![University](https://img.shields.io/badge/TUIASI-AC-blue?style=for-the-badge&logo=c-sharp)](https://ac.tuiasi.ro/)
[![Course](https://img.shields.io/badge/Curs-Inteligența_Artificială-orange?style=for-the-badge)](https://ac.tuiasi.ro/)
[![Algorithm](https://img.shields.io/badge/Algoritm-NSGA--II-green?style=for-the-badge)](https://en.wikipedia.org/wiki/Multi-objective_optimization)

> **Proiect universitar** Facultatea de Automatică și Calculatoare, Iași.  
> Specializarea: TI.

---

## 📖 Descrierea Problemei

Acest proiect abordează o problemă complexă de optimizare multi-obiectiv în contextul unui restaurant de tip Fast-Food (ex. McDonald’s). Scopul este de a găsi echilibrul perfect între resursele umane și satisfacția clienților.

Avem două **obiective contradictorii** care trebuie minimizate simultan:

1.  💸 **Minimizarea Costurilor (COST):** Utilizarea unui număr cât mai mic de angajați pentru a reduce cheltuielile salariale.
2.  ⏳ **Minimizarea Timpului de Așteptare (TIME):** Asigurarea unui număr suficient de angajați pentru a servi clienții rapid.

### ⚖️ Relația Contradictorie
* **Puțini angajați** 📉 Costuri mici ➡ 📈 Timp mare de așteptare.
* **Mulți angajați** 📈 Costuri mari ➡ 📉 Timp mic de așteptare.

**Scop:** Identificarea setului de soluții **Pareto-optime** care reprezintă cel mai bun compromis.

---

## 🧬 Despre Algoritmul NSGA-II

**NSGA-II (Non-dominated Sorting Genetic Algorithm II)** este un algoritm evolutiv avansat utilizat pentru probleme multi-obiectiv. Acesta ne permite să generăm o populație de soluții diverse, fără a reduce problema la o singură funcție de fitness ponderată.

### Concepte Cheie Utilizate:

* 🌀 **Crowding Distance:** Asigură diversitatea soluțiilor, prevenind aglomerarea lor într-o singură zonă a frontului Pareto.
* 🏆 **Elitism:** Garantează că cele mai bune soluții găsite nu sunt pierdute de la o generație la alta (părinții concurează cu copiii).
* 📊 **Fast Non-Dominated Sort:** Ierarhizează populația în "Fronturi Pareto". Frontul 1 (F1) conține soluțiile care nu sunt dominate de nimeni (cele mai bune).
* ⚔️ **Dominance Comparer:** Mecanismul care decide dacă soluția A este "strict mai bună" decât soluția B.

---

## 📂 Arhitectura Proiectului

Structura codului este organizată modular pentru claritate și extensibilitate:

| Modul | Descriere |
| :--- | :--- |
| **`Domain`** | Conține entitățile de bază (`Individual`) și constrângerile problemei (`OptimizationProblem`). |
| **`NSGA-II`** | "Creierul" algoritmului. Include logica de sortare, calculul distanței de aglomerare și crearea fronturilor. |
| **`Infrastructure`** | Implementarea concretă a funcțiilor obiectiv specifice Fast-Food (`FastFoodFitnessEvaluator`). |
| **`Application`** | Punctul de intrare. Coordonează componentele și rulează simularea. |

### Operatori Genetici Implementați

* 🧬 **Mutație:** `UniformMutation` (modificări aleatorii ale planului orar în funcție de rata de mutație).
* ❌ **Încrucișare (Crossover):** `AritmeticCrossover` (combină genele a doi părinți pentru a crea descendenți).
* 🎯 **Selecție:** `TournamentSelection` (selectează cei mai buni indivizi pentru reproducere).

---

## 📊 Rezultate și Concluzii

În urma rulării simulărilor, algoritmul nu oferă o soluție unică, ci un **Front Pareto**:

1.  **Extrema Economică:** Costuri minime (~870 RON), dar cu timpi de așteptare foarte mari (Penalizare > 1500).
2.  **Extrema Calitativă:** Timp de așteptare zero (Penalizare 0.00), dar cu costuri duble (~1875 RON).
3.  **Compromisul:** Soluțiile intermediare unde algoritmul a "învățat" tiparul cererii de clienți, alocând personal suplimentar doar la orele de vârf.

> **Concluzie:** Algoritmul NSGA-II demonstrează că nu există o organizare perfectă absolută, ci o serie de decizii manageriale bazate pe bugetul disponibil vs. standardul de calitate dorit.

---

## 👥 Echipa de Proiect

| Student | Rol și Contribuții |
| :--- | :--- |
| **Ciobanu Maria-Denisa** | 🏗️ Arhitectură (`Domain`), 🧬 Operatori (Mutație, Selecție), Organizare Interfețe. |
| **Tăbușcă Codrina-Florentina** | 🧠 Logică Core NSGA-II (Sortare, Distanță, Comparatori), ❌ Încrucișare. |
| **Lupu George** | ⚙️ Infrastructură (`FastFoodEvaluator`), 🏃‍♂️ Runner, Simulare date & Rulare. |

---

<div align="center">

**Universitatea Tehnică “Gheorghe Asachi” din Iași** *Facultatea de Automatică și Calculatoare* 2024 - 2025

</div>
