# Snake


## Uživatelská část

Cílem hry je pomoci hadovi sníst co nejvíce jablek. Had se pohybuje sám, jeho směr můžete
změnit pomocí tlačítek se šipkami (Nelze nastavit opačný směr). Na poli se vždy objeví pouze
jedno jablko. Když ho had sní, jeho délka se zvýší o 1 buňku a objeví se další jablko na
jakémkoli jiném místě, kam se potřebujete dostat. Nemůžete narazit do zdí a kousnout si ocas,
jinak prohrajete. Za každé jablko, které jíte, získáte body: 1 bod za červené jablko a 5 bodů za
žluté. Šance, že se objeví žluté jablko, je 17%. Po kliknutí na tlačítko Play se spustí nová hra.
Text Score ukazuje počet bodů, které jste získali.


## Programátorská část

Při spuštění se vygeneruje had a pole dané velikosti, рočáteční směr je vpravo (1, 0),
рočáteční stav - start. Když uživatel klikne na tlačítko, hra přejde do stavu game. Objeví se
jablko, pokud to není první hra, starý had bude odstraněn a objeví se nový, a také začne
časovač. Pro každý interval časovače se zkontroluje, zda had narazil do zdi nebo sežral ocas
(pokud ano, hra končí), zda snědl jablko, a poté se pohne o 1 buňku daným směrem. Toto se
bude opakovat, dokud nejsou splněny tyto podmínky a hra neskončí. Když hra skončí, jablko je
odstraněno z pole (pokud existovalo), časovač se zastaví, zobrazí se konečné skóre, po kterém
se hodnota proměnné score vynuluje. Při příštím stisknutí tlačítka Play se všechno zopakuje.

### Jablka

Abychom jablko vytvořili, potřebujeme vědět, o jaký typ se bude jednat a kde se bude nacházet.
Abychom zjistili druh jablka, použijeme funkci ***appleType()***, která vygeneruje náhodné číslo od
1 do 100 a uloží ho do proměnné ***fruitNumber***. Pokud je toto číslo menší nebo rovno šanci na
žluté jablko (proměnná ***fruitChance***), vytvoří se žluté jablko, jinak červené.
Abychom umístili jablko na volnou buňku pole, pomoci funkci ***freePositions()*** vybereme
náhodnou pozici ze seznamu volných pozic ***freePos***. (Pokud nejsou, hra přejde do stavu END)
K tomu existuje třída ***Position(int x, int y, bool free)***, kde x a y jsou souřadnice buňky a free
ukazuje, zda je buňka volná. Když had sní jablko (funkce ***eatApple()***), jeho délka se zvětší o 1,
skóre se zvýší v závislosti na barvě jablka, staré jablko se odstraní a vytvoří se nové.

### Had

Had je reprezentován polem. Na začátku hry se vygeneruje počet buněk určený proměnnou
***snakeSize***. Had je generován pomocí funkce ***generateSnake(int i)***, kde i je číslo nové buňky v
poli. Toto číslo zjistíme pomocí proměnné ***snakeLength***, která se nejprve rovná ***snakeSize*** a
poté se po snězení každého jablka zvýší o 1. Had se pohybuje pomoci funkcí ***move()***, který
přesune každou buňku na místo buňky s předchozím číslem v poli a hlavu (***snake[0]***) posune o
jednu buňku ve směru pohybu. Také během této funkce se stará pozice poslední buňky je
označena jako volná a nová pozice hlavy jako obsazená.

### Směr

Objekt direction třídy ***Direction(int row, int column)*** existuje, aby se had pohyboval správným
směrem, dokud se parametry objektu nezmění. Nejprve je směr nastaven doleva, protože had
je generován ve směru pohybu, ale po počátečním vygenerování hada se směr změní doprava.
Po dekódování signálu stisknuté klávesy na klávesnici funkcí ***click(object sender,
KeyEventArgs e)*** funkce ***ChangeDirection(Direction direction, string s)*** zkontroluje pomocí
funkce ***checkOppositeDirection(Direction d, Direction nd)***, zda je nový směr opačný.
Když had narazí do zdi nebo ocasu, hra přejde do stavu END: оbjeví se tlačítko Play, časovač se
zastaví, hadi a jablka budou odstraněni, skóre se vynuluje. Vše se spustí znovu po stisknutí
tlačítka Play
