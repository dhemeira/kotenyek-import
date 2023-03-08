# Használati útmutató

A bekezdésekhez gyors elérési linkeket itt találod:
![Tartalom táblázat kép](https://github.com/dhemeira/kotenyek-import/blob/master/readme_images/tableofcontents.png?raw=true)

Első használat előtt javasolt végigolvasni ezen útmutatót, és később felmerülő kérdés esetén újra elolvasni a kérdésre vonatkozó részt.

### Bejelentkezés

![Bejelentkezés kép](https://github.com/dhemeira/kotenyek-import/blob/master/readme_images/bejelentkezes.png?raw=true)

Az alkalmazás első megnyitásakor, vagy ha nem vagy bejelentkezve ez az ablak fogad.
Nem szükséges minden megnyitáskor bejelentkezni abban az esetben, ha használat után csak bezárod az oldalt és nem jelentkezel ki.

- Először add meg az _`Oldal URL`_-jét, vagy más névet linkjét.
  - Ennek az ajánlott formája a következő: `weboldal.hu`
  - Ennek 2 módja van:
    <ol type="1">
      <li>csak egyszerűen írd be vagy másold be a szövegmezőbe</li>
      <li>a legördülő listából válaszd ki a már előzetesen megadott linket</li>
    </ol>
  - Ha csak egyszerűen kimásolnád, akkor a `https://weboldal.hu/` formátum is támogatva van
  - Jelenleg csak `.hu` vagy `.com` használata lehetséges
- Ezután add meg az oldalon is használt _`felhasználóneved`_
- Majd az oldalon is használt _`jelszavad`_
- Legvégül nyomj rá a `Bejelentkezés` gombra

**Használat után _nem_ szükséges kijelentkezni!**

Ha mindent helyesen adtál meg és az oldal is megfelelően működik, akkor sikeresen bejelentkeztél, és ez az ablak bezáródik.

Ha valamit hibásan adtál meg, vagy a szerverrel nem vehető fel a kapcsolat, akkor erről egy hibaüzenet értesít. Ha a hibaüzenet szövege `Ismeretlen hiba`, akkor ezt rögtön jelezd, ellenkező esetben módosítsd az adatokat a hibaüzenetnek megfelelően.

**Ha háromszor hibásan adod meg a jelszavad, az oldal 10 percig nem enged újrapróbálkozni.**

A felléphető hibákról, hibaüzenetekről a [Hibák és hibaüzenetek](#hibák-és-hibaüzenetek) bekezdésben olvashatsz.

## Az alkalmazás használata

![Alkalmazás kép](https://github.com/dhemeira/kotenyek-import/blob/master/readme_images/alkalmazas.png?raw=true)

Sikeres bejelentkezést követően ez az ablak fogad.

A termékek hozzáadás utáni szerkesztése **CSAK A MENTETT `.CSV` FÁJLBAN vagy feltöltés után az oldalon lehetséges**, ezért győződj meg róla, hogy mindent megfelelően töltöttél ki, mielőtt hozzáadnád a terméket.

A piros csillaggal jelölt mezők kitöltése kötelező, a többi a termék típusától függően ajánlott.

<b style="color:red">
HA NEM MENTED AZ IMPORT FÁJLT A PROGRAM BEZÁRÁSA ELŐTT, AKKOR MINDEN EDDIG FELVITT ADAT ELVESZIK!
</b>

### Képek

Kép hozzáadása **nem** kötelező.

A kép mező nem szerkezthető, hibázási lehetőségek elkerülése érdekében. Emiatt győződj meg róla hogy a **megfelelő képet választod ki**.

Kép feltöltésére A **`Képek feltöltése`** gomb szolgál. Itt egyszerre 1 vagy akár több kép is megadható. A termék listához adása előtt további képek is feltölthetők az adott termékhez.

**FONTOS:** A kép feltöltődik a weboldalra abban a pillanatban amint hozzáadod a képet, akkor is, ha végül nem töltöd fel a terméket az oldalra. Emiatt csak olyan képeket válassz, amit biztosan fel szeretnél tölteni az oldalra.

(Ha véletlenül mégis feltöltöttél egy képet amit nem akartál, akkor az sehol sem jelenik meg az oldalon automatikusan, és az oldal adminfelületén törölhető a kép.)

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

A fent említett speciális karakterekről, jelölésekről részletesebben a leírás végén vagy [ide kattintva](#az-import-fájl-csv-fájl-kinézete-speciális-karakterek-jelölése) olvashatsz.

### Ár

Ár megadása **kötelező**.

A termék ára forintban megadva. **Csak** a szám értéket kell és szabad tartalmaznia, pénznemet **NEM**.

példa:

- Ár: `1234`

### Kategória

Kategória megadása **kötelező**.

A megjelenő mezők közül pipáld be azokat, amikbe a termék tartozik.
Ha egy olyan kategóriát választasz, ami egy másik kategória alkategóriája (pl. Derekas kötény a Kötény alkategóriája), akkor a szülő kategória automatikusan kiválasztásra kerül.

### Cikkszám

Cikkszám megadása **kötelező**.

A cikkszám egy tetszőleges számú betűket és számokat is tartalmazó jelölés.

- A cikkszám nagybetűsen kerül mentésre, de erre nem kell külön odafigyelni, ha kisbetűsen adod meg, akkor átkonvertálódik
- Ha meg van adva a termék színe és a kapható színek, a cikkszámnak kötelezően tartalmaznia kell a szín 3 betűs angol rövidítését a cikkszám végén
  - például: `MINTA123BLK` vagy `MINTA123WHT`, ami nem más mint a minta kötény fekete vagy fehér színben
  - ez azért fontos, mert a további elérhető színeket a cikkszám, a szín és a kapható színek információ hármas alapján generálja a rendszer. Ha ezek közül valamelyik hibásan van megadva, a termékhez kapcsolódó további színek is hibásan fognak megjelenni

---

A további mezők kitöltése **nem** kötelező:

### Mosási útmutató

`WIP`

### Hossz

A termék hossza centiméterben (cm) megadva. Mértékegységet odaírni nem kell és nem is szabad.

### Szélesség

A termék szélessége centiméterben (cm) megadva. Mértékegységet odaírni nem kell és nem is szabad.

### Szín

A termék színe. Ha a listán nem található színű terméket szeretnél hozzáadni, akkor előbb az oldalon létre kell hoznod a színt, ahol a `név` a szín neve, a `slug` pedig a szín 3 betűs angol rövidítése.

Ha meg van adva kapható szín, akkor a színnek azok közül kell kiválasztásra kerülnie.

### Kapható színek

A termék összes kapható szín variációja. Ha a listán nem található színt szeretnél hozzáadni, akkor előbb az oldalon létre kell hoznod a színt a kapható színek között, ahol a `név` a szín neve, a `slug` pedig a szín 3 betűs angol rövidítése.

**Figyelem**: A kapható színek között szerepelnie kell az adott termék színének is.

### Hozzáadás és Új szín

Ennek a gombnak a megnyovásával hozzáadod az aktuális terméket a listához. Emellett a program előkészíti a szövegmezőket a termék következő színének felviteléhez.

- Ha mégsem új színt szeretnél megadni hanem egy teljesen új terméket, akkor csak egyszerűen változtasd meg a szövegmezőkben maradt adatokat tetszés szerint
- Ez a gomb a `Hozzáadás és Új termék` gombtól annyiban tér el, hogy nem üríti ki az összes szövegmezőt, csak azokat amik színenként is eltérnek. Ezen kívül semmi eltérés nincs a kettő között!
- **Figyelem**: Ez a gomb nem menti el a terméket, csak felveszi a mentésre kész termékek listájára. `Import fájl mentése` nélküli kilépés esetén ezek törlődnek!

### Hozzáadás és Új termék

Ennek a gombnak a megnyovásával hozzáadod az aktuális terméket a listához. Emellett a program előkészíti a szövegmezőket a következő termék felvitelére.

- Ha mégsem új terméket szeretnél feltölteni hanem egy új színt, akkor csak egyszerűen add meg az új színű termékhez tartozó adatokat.
- Ez a gomb a `Hozzáadás és Új szín` gombtól annyiban tér el, hogy kiüríti az összes szövegmezőt. Ezen kívül semmi eltérés nincs a kettő között!
- **Figyelem**: Ez a gomb nem menti el a terméket, csak felveszi a mentésre kész termékek listájára. `Import fájl mentése` nélküli kilépés esetén ezek törlődnek!

### Import fájl mentése

Ez a gomb felel a termékek fájlba mentésére.
Egyszerűen válaszd ki a mentés helyét a felugró ablakban, és add meg a fájl nevét.
**Az oldalra feltöltés előtt ellenőrizd a generált .csv fájl tartalmát!** Ehhez segítséget a leírás végén vagy [ide kattintva](#az-import-fájl-csv-fájl-kinézete-speciális-karakterek-jelölése) kaphatsz.

### Kijelentkezés

Ezzel a gombbal kijelentkezel a fiókodból. Ezt csak akkor érdemes használni, ha felhasználói fiókot szeretnél váltani.

Az alkalmazás használata után **NEM** szükséges kilépni.

Ha véletlenül kiléptél és mentetlen termékeid vannak, próbálj meg bejelentkezni, ameddig nem zárod be az alkalmazást, addig nagy eséllyel megmaradnak a termékek!

## A kész import fájl feltöltésének módja

`WIP`

## Példák

`WIP`

## Hibák és hibaüzenetek

`WIP`

## Az import fájl (.csv fájl) kinézete, speciális karakterek jelölése

`WIP`

`<sup>2</sup>`
`\n`
| Minta | Táblázat |
| ----------- | ----------- |
| első | sor |
| második | sor |
