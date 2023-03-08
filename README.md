# Használati útmutató

A bekezdésekhez gyors elérési linkeket itt találod:
![Tartalom táblázat kép](https://github.com/dhemeira/kotenyek-import/blob/master/readme_images/tableofcontents.png?raw=true)

### Bejelentkezés

![Bejelentkezés kép](https://github.com/dhemeira/kotenyek-import/blob/master/readme_images/bejelentkezes.png?raw=true)

Az alkalmazás első megnyitásakor, vagy ha nem vagy bejelentkezve ez az ablak fogad.
Nem szükséges minden megnyitáskor bejelentkezni abban az esetben, ha használat után csak bezárod az oldalt és nem jelentkezel ki.

- Először add meg az **Oldal URL**-jét, vagy más névet linkjét.
  - Ennek az ajánlott formája a következő: **weboldal.hu**
  - Ennek 2 módja van:
    1. csak egyszerűen írd be vagy másold be a szövegmezőbe
    2. a legördülő listából válaszd ki a már előzetesen megadott linket
  - Ha csak egyszerűen kimásolnád, akkor a https://weboldal.hu/ formátum is támogatva van
  - Jelenleg csak .hu vagy .com használata lehetséges
- Ezután add meg az oldalon is használt **felhasználóneved**
- Majd az oldalon is használt **jelszavad**
- Legvégül nyomj rá a **"Bejelentkezés"** gombra

**Használat után _nem_ szükséges kijelentkezni!**

Ha mindent helyesen adtál meg és az oldal is megfelelően működik, akkor sikeresen bejelentkeztél, és ez az ablak bezáródik.

Ha valamit hibásan adtál meg, vagy a szerverrel nem vehető fel a kapcsolat, akkor erről egy hibaüzenet értesít. Ha a hibaüzenet szövege **"Ismeretlen hiba"**, akkor ezt rögtön jelezd, ellenkező esetben módosítsd az adatokat a hibaüzenetnek megfelelően.

**Ha háromszor hibásan adod meg a jelszavad, az oldal 10 percig nem enged újrapróbálkozni.**

A felléphető hibákról, hibaüzenetekről a [Hibák és hibaüzenetek](#hibak) bekezdésben olvashatsz.

## Az alkalmazás használata

![Alkalmazás kép](https://github.com/dhemeira/kotenyek-import/blob/master/readme_images/alkalmazas.png?raw=true)

Sikeres bejelentkezést követően ez az ablak fogad.

A termékek hozzáadás utáni szerkesztése **CSAK A MENTETT .CSV FÁJLBAN vagy feltöltés után az oldalon lehetséges**, ezért győződj meg róla, hogy mindent megfelelően töltöttél ki, mielőtt hozzáadnád a terméket.

A piros csillaggal jelölt mezők kitöltése kötelező, a többi a termék típusától függően ajánlott.

### Képek

Kép hozzáadása **nem** kötelező.

A kép mező nem szerkezthető, hibázási lehetőségek elkerülése érdekében. Emiatt győződj meg róla hogy a **megfelelő képet választod ki**.

Kép feltöltésére A **"Képek feltöltése"** gomb szolgál. Itt egyszerre 1 vagy akár több kép is megadható. A termék listához adása előtt további képek is feltölthetők az adott termékhez.

**FONTOS:** A kép feltöltődik a weboldalra abban a pillanatban amint hozzáadod a képet, akkor is, ha végül nem töltöd fel a terméket az oldalra. Emiatt csak olyan képeket válassz, amit biztosan fel szeretnél tölteni az oldalra.

(Ha véletlenül mégis feltöltöttél egy képet amit nem akartál, akkor az sehol sem jelenik meg az oldalon autómatikusan, és az oldal adminfelületén törölhető a kép.)

### Név

Név megadása **kötelező**.

A termék neve. Az egyedi termék nevek garantálása érdekében a névnek tartalmazni kell a cikkszámot is. Ennek a következő formában kell történnie:

- `Termék neve (Cikkszám)`
- azaz: `Terméknév` **`SZÓKÖZ`** **`ZÁRÓJEL`** `Cikkszám` **`ZÁRÓJEL`**

Kis és nagybetűkre a termék nevénél nem kell odafigyelni, ez magától nagybetűsen kerül mentésre.

### Leírás

Leírás megadása **nem** kötelező.

Ez a termék leírása. Az `Enterek` (sortörések) az itt megadott leírásnak megfelelően meg fognak jelenni a leírásban is. Ezek az `Enterek` az import fájlban `\n`-el vannak jelölve. A végső A szövegdobozban egyszerre 10 sor látható, ennek ellenére több sor is megadható, és a `Fel` vagy `Le` nyilakkal lehet eljutni a rejtett sorokhoz.

Ezen felül elérhető a szövegdoboz alatt 2 további gomb is, ezek az x<sup>2</sup> (valami a négyzeten) és az x<sup>3</sup> (valami a köbön) jelek beszúrására szolgálnak.

- Ezek a szövegdobozban a következőképp jelennek meg: `<sup>2<sup>` vagy `<sup>3</sup>`
- Azaz ha például négyzetmétert (m<sup>2</sup>) szeretnénk írni, annak a következőképp kell megjelennie a szövegdobozban: `m<sup>2</sup>`
- Kézzel bevihető bármilyen más hatvány is, legyen ez szám, betű, vagy akár hosszabb szöveg is, ehhez a fentebb feltüntetett formát szükséges használni: `<sup>A szám, betű, vagy szöveg ami a felső indexben megjelenjen</sup>`

A fent említett speciális karakterekről, jelölésekről részletesebben a leírás végén vagy [ide kattintva](#import-fajl) olvashatsz.

### Ár

Ár megadása **kötelező**.

A termék ára. **Csak** a szám értéket kell és szabad tartalmaznia, pénznemet **NEM**.

példa:

- Ár: `1234`

### Kategória

Kategória megadása **kötelező**.

### Cikkszám

Cikkszám megadása **kötelező**.

---

A további mezők kitöltése **nem** kötelező.

### Mosási útmutató

### Hossz

### Szélesség

### Szín

### Kapható színek

### Hozzáadás és Új szín

### Hozzáadás és új termék

### Import fájl mentése

### Kijelentkezés

## Példák

## <a name="hibak">Hibák és hibaüzenetek</a>

## <a name="import-fajl">Az import fájl (.csv fájl) kinézete, speciális karakterek jelölése</a>
