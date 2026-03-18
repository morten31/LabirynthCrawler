Oto treść zadania sformatowana w języku Markdown:

# Etap 1: Podstawowy program gry

## Cel zadania
Stworzenie konsolowej wersji gry RPG, w której:
1. Gracz porusza się po jednym pomieszczeniu.
2. Gracz zbiera różne przedmioty.

---

## Wymagania funkcjonalne

### 1. Pomieszczenie:
*   Plansza gry to prostokąt o stałym rozmiarze **20x40** pól.
*   Każda komórka planszy może być:
    *   pusta (` `),
    *   ścianą (`|`),
    *   zawierać gracza (`@`),
    *   zawierać wiele przedmiotów (dowolny symbol, zależny od przedmiotów).
*   Pozycja początkowa gracza: **(0, 0)**.
*   Na tym etapie nie jest wymagana generacja labiryntu ani rozmieszczenia przedmiotów. Przedmioty i ściany mogą być ułożone w predefiniowany sposób.

### 2. Gracz:
*   Porusza się w czterech kierunkach (kontrola literami `W`, `S`, `A`, `D`).
*   Nie może wyjść poza granice planszy.
*   Nie może przechodzić przez ściany.
*   Ma różne atrybuty, takie jak siła, zręczność, zdrowie, szczęście, agresja i mądrość.
*   Ma dwie ręce, w każdej może umieścić przedmiot.

### 3. Przedmioty:
*   W pomieszczeniu rozmieszczone są zdefiniowane w kodzie przedmioty.
*   Są przynajmniej trzy rodzaje broni, w tym jedna dwuręczna. Broń ma wartość obrażeń.
*   Są przynajmniej trzy rodzaje nieużywalnych przedmiotów.
*   Są dwa rodzaje waluty: monety i złoto.
*   Każda broń ma obrażenia, które zadaje przy użyciu (na tym etapie jeszcze nie implementujemy użycia broni, tylko zakładanie).
*   Jeśli gracz stanie na polu z przedmiotem, to może go podnieść naciskając przycisk `E`.

### 4. Wyświetlanie stanu gry:
*   Plansza jest rysowana w konsoli.
*   Obok planszy wyświetlane są:
    *   ekwipunek,
    *   aktualnie używane przedmioty,
    *   jeśli gracz jest na polu, na którym jest przedmiot, to informacja o tym,
    *   aktualne wartości atrybutów gracza,
    *   liczba zebranych monet i złota.

### 5. Ekwipunek
*   Gracz może zarządzać swoim ekwipunkiem, t.j.:
    *   wyrzucać przedmioty na ziemię,
    *   wyciągać i wkładać przedmioty do obu rąk (poprawna obsługa broni dwuręcznej!).

---

## Uwagi
1.  Kod musi być zorientowany obiektowo.
2.  Dodanie nowego rodzaju: broni, przedmiotu, pola na planszy itp. powinno być łatwe.
3.  Zabronione jest używanie rozpoznawania typów w czasie wykonywania programu (np. `is`, `as`, `typeof`, `RTTI` w C++) oraz używanie wyliczeń w celu rozpoznania konkretnego typu obiektu.
4.  Za każde użycie niedozwolonej funkcjonalności języka -1p. oraz konieczność poprawy w późniejszych etapach.
5.  Plansza powinna być wyświetlana w taki sposób, aby wrażenie z rozgrywki było pozytywne (w szczególności zadbaj o to, aby obraz nie przeskakiwał po każdym ruchu oraz, aby zmiana stanu gry odzwierciedlana była w miarę szybko).