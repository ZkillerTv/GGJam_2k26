# Guida implementazione scena Stazione

Dopo aver creato la struttura (Tools → Stazione → Crea struttura scena stazione), segui questa checklist in ordine.

---

## Stato scena livello_empatia (modifiche applicate)

**Vista 2D:** La Main Camera è stata impostata su **ortografica** (orthographic: 1) e posizione (0, 0, -10), così la scena si vede in 2D (piano XY, Z = profondità).

**Posizioni per layout 2D:**
- **Panchina_Centrale:** (0, 0, 0) – centro, con prefab bench ✓  
- **Tabellone_Treni:** (6, 0, 0) – a destra  
- **Biglietteria_InfoPoint:** (9, 0, 0) – destra  
- **Binari_Pensiline:** (0, -1.5, **0**) – sfondo con figlio **Background_Sprite** (Order in Layer **-1**), stesso Z degli altri  
- **NPC_Vecchietta:** (-1.5, 0, 0) – vicino alla panchina  
- **NPC_Manager:** (5, 0.5, 0) – vicino al tabellone  
- **NPC_Terzo:** (2, -1, 0) – centro-destra  

**Cosa manca / da correggere:**
1. **Tabellone_Treni:** sotto c’è il prefab **“air conditioner roof”** (condizionatore). Sostituiscilo con la tua **immagine 2D** (vedi sezione “Tabellone con immagine 2D” sotto).  
2. **NPC_Vecchietta, NPC_Manager, NPC_Terzo, NPC_Quarto:** hanno già un **quadrato colorato** come placeholder (Quad_Vecchietta arancio, Quad_Manager blu, Quad_Terzo verde, Quad_Quarto grigio). Puoi sostituirli con prefab/sprite personaggio quando vuoi.  
3. **Binari_Pensiline:** ha già un figlio **Background_Sprite** con Order in Layer -1 (dietro a tutto). È usato uno sprite placeholder; sostituisci con la tua immagine binari/pensiline (vedi sezione “Binari_Pensiline come background” sotto.  
4. **Biglietteria_InfoPoint:** è un empty, nessun modello. Va bene come punto di destinazione; opzionale aggiungere un chiosco/edicola.  
5. **Punto_Accompagno** (figlio di NPC_Terzo): posizionalo vicino a Biglietteria (es. 9, 0, 0) se non lo è già.  
6. **Canvas** (sotto _Stazione): è in **Screen Space - Camera** con Main Camera e Plane Distance 10, così l’UI è davanti alla scena senza coprirla. Il Panel è trasparente. Aggiungi i pulsanti (Ascolta / Offri / Stai) come figli del Canvas. Riferimento: [Canvas – Unity UGUI](https://docs.unity3d.com/Packages/com.unity.ugui@2.0/manual/UICanvas.html) (Render Modes: Overlay, Screen Space - Camera, World Space).

---

## Tabellone con immagine 2D

Se hai un’immagine 2D per il tabellone partenze/arrivi:

1. **Rimuovi il prefab sbagliato**  
   In Unity: seleziona **Tabellone_Treni** nella Hierarchy, espandi il nodo, seleziona il figlio *“air conditioner roof Prefab”* e premi **Canc** (o tasto destro → Delete). Così resta solo l’empty Tabellone_Treni.

2. **Importa l’immagine come Sprite**  
   - Trascina la tua immagine in `Assets` (es. `Assets/Immagini/Stazione/` o una cartella dedicata).  
   - Seleziona il file immagine nel Project.  
   - Nell’**Inspector**: **Texture Type** = **Sprite (2D and UI)**.  
   - Clicca **Apply**.

3. **Aggiungi il tabellone in scena**  
   - Nella Hierarchy: tasto destro su **Tabellone_Treni** → **Create Empty**.  
   - Rinomina il figlio (es. **Tabellone_Sprite**).  
   - Con il figlio selezionato: **Add Component** → **Sprite Renderer**.  
   - Nel campo **Sprite** trascina la tua immagine dal Project (oppure usa il cerchietto e scegli lo sprite).  
   - Regola **Transform** (Position, Scale) per dimensioni e posizione. Con vista 2D, di solito Z = 0.  
   - Se qualcosa copre il tabellone: nello Sprite Renderer aumenta **Order in Layer** (es. 1 o 2).

La scena è già in vista 2D (camera ortografica), quindi lo sprite sarà visibile sul piano della scena.

---

## Binari_Pensiline come background

Lo sfondo usa **Order in Layer** (non la Z): tutto è su **Z=0**, il background sta dietro perché il figlio **Background_Sprite** ha **Order in Layer = -1**.

1. **Sostituire lo sprite**  
   Come per il tabellone: trascina l’immagine in Assets, selezionala, **Texture Type** = **Sprite (2D and UI)** → Apply.

2. **Aggiungi lo sfondo in scena**  
   - Tasto destro su **Binari_Pensiline** → **Create Empty**.  
   - Rinomina (es. **Background_Binari**).  
   - **Add Component** → **Sprite Renderer**, assegna la tua immagine nel campo **Sprite**.

3. **Fallo stare dietro a tutto**  
   - Nello **Sprite Renderer** imposta **Order in Layer** = **-1** (o altro numero negativo). Così viene disegnato dietro panchina, NPC e tabellone (che possono stare su 0, 1, 2…).

4. **Posizione e scala**  
   - **Position**: già impostata sul nodo Binari_Pensiline (0, -1.5, **1**) così sta dietro a tutto. Puoi lasciare il figlio a (0,0,0) locale o spostarlo per centrare lo sfondo.  
   - **Scale**: regola in base alla risoluzione dell’immagine e alla camera (orthographic size 5) così copre l’inquadratura.

Se hai più layer (es. cielo, binari, primo piano), usa Order in Layer diversi (es. -2, -1, 0).

---

## Sprite sheet 4 personaggi (preview.jpg)

Hai uno sprite sheet in **Assets/Sprites/preview.jpg** con **4 personaggi**: 3 righe di maschi (6 pose ciascuno) e 1 riga di 4 femmine (3 pose ciascuna). Per usarlo con i 4 NPC:

1. **Import già impostato**  
   La texture è già in **Sprite Mode = Multiple** (Texture Type = Sprite). Se Unity non l’ha ancora applicato, seleziona `Assets/Sprites/preview.jpg` → Inspector → **Texture Type** = **Sprite (2D and UI)** → **Sprite Mode** = **Multiple** → Apply.

2. **Tagliare lo sheet (Sprite Editor)**  
   - Seleziona **preview.jpg** nel Project.  
   - Clicca **Sprite Editor** (in Inspector).  
   - Taglia le celle: **Slice** → **Grid By Cell Count** (es. 6 colonne × 5 righe) oppure **Grid By Cell Size** con larghezza/altezza in pixel di una cella. La riga in basso ha 12 celle (4 femmine × 3 pose).  
   - Rinomina gli sprite (es. `vecchietta_0`, `manager_0`, `terzo_0`, `quarto_0` per la prima pose).  
   - Chiudi lo Sprite Editor → **Apply**.

3. **Assegnare gli sprite agli NPC**  
   - **NPC_Vecchietta** → figlio **Quad_Vecchietta** → Sprite Renderer → **Sprite** = femmina vestito marrone (prima del quartetto in basso).  
   - **NPC_Manager** → **Quad_Manager** → Sprite = uomo biondo giacca nera (prima riga).  
   - **NPC_Terzo** → **Quad_Terzo** → Sprite = uomo capelli scuri camicia blu (seconda riga).  
   - **NPC_Quarto** → **Quad_Quarto** → Sprite = uomo biondo camicia blu (terza riga) oppure un’altra femmina.

---

## 1. AMBIENTE (prefab e modelli)

| GameObject | Cosa fare |
|------------|-----------|
| **Panchina_Centrale** | Assegna un prefab panchina (es. da POLYGON city pack: bench prefab). Posizionala al centro. Qui si siede la vecchietta. |
| **Tabellone_Treni** | Usa la tua **immagine 2D** come sprite (vedi sezione “Tabellone con immagine 2D” sopra). Oppure pannello UI (TextMeshPro) che aggiorni raramente. |
| **Binari_Pensiline** | Usalo come **background** 2D: immagine sprite (binari/pensiline) con **Order in Layer** negativo (es. -1) così sta dietro a tutto. Vedi “Binari_Pensiline come background” sopra. |
| **Biglietteria_InfoPoint** | Un punto/area (empty + collider o marker). Il Terzo va “accompagnato” qui per la soluzione giusta. |

**Suggerimento:** Usa prefab da `Assets/POLYGON city pack/Prefabs/` (bench, walls, etc.) e trascinali come figli dei nodi sopra. Mantieni la gerarchia _Stazione → Ambiente → ….

---

## 2. NPC (modelli + posizioni)

| NPC | Cosa fare |
|-----|-----------|
| **NPC_Vecchietta** | Assegna un prefab personaggio “anziana” (o placeholder cubo). Mettila vicino alla panchina. Su **Orologio_Visibile** aggiungi un modello orologio (polso o portatile). **Punto_Seduta** = Transform sulla panchina dove il player “si siede” (stai). |
| **NPC_Manager** | Prefab personaggio “manager”. Posizionalo vicino al tabellone, in atteggiamento “al telefono”. **Punto_Guardia_Tabellone** = Transform dove guarda (look at tabellone). |
| **NPC_Terzo** | Prefab personaggio. In mano o in UI: “telefono con batteria 3%”. **Punto_Accompagno** = Transform alla biglietteria (stesso punto di Biglietteria_InfoPoint o figlio). |
| **NPC_Quarto** | Opzionale: lascia empty o aggiungi un quarto personaggio. |

**Importante:** Ogni NPC avrà uno **script di dialogo/logica** (vedi sotto). Per ora basta modello + posizione.

---

## 3. SCRIPT DA SCRIVERE (priorità)

### 3.1 TimeController (alta priorità)

- **Dove:** Sul GameObject `Sistema/TimeController`.
- **Cosa fa:** Il tempo (Time.timeScale o un timer di “turno”) avanza **solo** quando il giocatore compie un’azione (Ascolta / Offri / Stai su un NPC).
- **Implementazione minima:**  
  - `float timeScale = 0f` quando nessuna azione.  
  - Quando il player sceglie un’azione su un NPC → `timeScale = 1f` per X secondi (o un “tick”), poi torna a 0.  
  - Oppure: tempo a turni (ogni scelta = 1 tick).

---

### 3.2 InteractionManager (alta priorità)

- **Dove:** Sul GameObject `Sistema/InteractionManager`.
- **Cosa fa:** Gestisce le **3 tipi di interazione** (Ascolta, Offri, Stai) e quale NPC è selezionato.
- **Implementazione minima:**  
  - Raggio/click per selezionare un NPC.  
  - UI con 3 pulsanti: **Ascolta**, **Offri**, **Stai**.  
  - Al click: invia evento al NPC selezionato con il tipo (Listen / Offer / Stay).  
  - Opzionale: “Offri” può aprire un sottomenu (cosa offrire, es. telefono).

---

### 3.3 Script per ogni NPC (alta priorità)

Uno script per tipo di puzzle, assegnato al rispettivo NPC.

| Script | GameObject | Responsabilità |
|--------|------------|----------------|
| **NPC_VecchiettaBehaviour** | NPC_Vecchietta | Dialogo “Che ore sono?”. Se risposta = orario → loop. Se azione = **Stai** (e player vicino a Punto_Seduta) → dopo X secondi sblocca “Mio marito lavorava qui” e puzzle risolto. |
| **NPC_ManagerBehaviour** | NPC_Manager | Urla al telefono, animazione/voce. Se dici “calmati” o “riunione” → negativo. Se **Ascolta** + risposta “C’è un altro treno fra 5 minuti” (o Offri info orario) → chiude telefono, sospiro, risolto. |
| **NPC_TerzoBehaviour** | NPC_Terzo | Chiede caricabatterie. Se cerchi caricatore / ignori → fallisce. Se **Offri** il tuo telefono O **Stai** + accompagni fino a Punto_Accompagno → “Grazie… davvero.”, risolto. |

**Comune a tutti:**  
- Ricevere dall’InteractionManager: tipo azione (Ascolta/Offri/Stai).  
- Stato interno: non risolto / risolto.  
- Quando risolto: disattivare o far “partire” l’NPC (animazione treno / uscita di scena).

---

### 3.4 WinCondition (media priorità)

- **Dove:** Sul GameObject `Sistema/WinCondition`.
- **Cosa fa:** Controlla se **tutti** gli NPC sono in stato “risolto”. Quando sì: tutti se ne vanno (salgono su treno), stazione vuota → **vittoria** (carica scena successiva o mostra “Livello completato”).

---

### 3.5 Player / UI (media priorità)

- **Player:** Camera (o avatar) che si muove / punta agli NPC. Click o raggio per selezionare.
- **UI_Interazioni:** Pannello con 3 pulsanti (Ascolta, Offri, Stai). Si mostra quando un NPC è selezionato. Chiama InteractionManager.

---

## 4. FLUSSO CONSIGLIATO

1. **Ambiente:** Piazzare prefab panchina, tabellone, biglietteria (anche placeholder).
2. **TimeController:** Script minimo (tempo fermo → avanza solo su azione).
3. **InteractionManager:** 3 azioni + selezione NPC (anche senza grafica, solo log).
4. **Un NPC alla volta:** Prima Vecchietta (solo Stai), poi Manager (solo Ascolta con frase giusta), poi Terzo (Offri / Stai accompagno).
5. **WinCondition:** Quando tutti e 3 risolti → trigger vittoria.
6. **UI e polish:** Pulsanti, feedback, “il tempo avanza” visibile.

---

## 5. RIEPILOGO FILE DA CREARE

| Script | Cartella suggerita | Assegnato a |
|--------|--------------------|-------------|
| `TimeController.cs` | Script/Sistema/ | Sistema/TimeController |
| `InteractionManager.cs` | Script/Sistema/ | Sistema/InteractionManager |
| `WinCondition.cs` | Script/Sistema/ | Sistema/WinCondition |
| `NPC_VecchiettaBehaviour.cs` | Script/NPC/ | NPC/NPC_Vecchietta |
| `NPC_ManagerBehaviour.cs` | Script/NPC/ | NPC/NPC_Manager |
| `NPC_TerzoBehaviour.cs` | Script/NPC/ | NPC/NPC_Terzo |

(Opzionale: interfaccia `INPCInteractable` con `OnListen()`, `OnOffer()`, `OnStay()` che ogni NPC implementa.)

---

## 5b. COSA FARE ADESSO – SCRIPT (passi precisi)

Gli script sono già in **Script/Sistema/** e **Script/NPC/**. In Unity:

1. **Assegnare gli script ai GameObject**
   - **Sistema/TimeController** → Add Component → `TimeController`.
   - **Sistema/InteractionManager** → Add Component → `InteractionManager`.
   - **Sistema/WinCondition** → Add Component → `WinCondition`.
   - **NPC/NPC_Vecchietta** → Add Component → `NPC_VecchiettaBehaviour`.
   - **NPC/NPC_Manager** → Add Component → `NPC_ManagerBehaviour`.
   - **NPC/NPC_Terzo** → Add Component → `NPC_TerzoBehaviour`.

2. **Configurare i riferimenti in Inspector**
   - **NPC_Vecchietta** (con `NPC_VecchiettaBehaviour`): trascina **Punto_Seduta** (figlio dello stesso NPC) nel campo **Punto Seduta**.
   - **NPC_Terzo** (con `NPC_TerzoBehaviour`): trascina **Punto_Accompagno** (figlio dello stesso NPC) nel campo **Punto Accompagno**.
   - **WinCondition**: puoi lasciare vuoto l’array **Npc Behaviours** (lo script trova da solo i 3 NPC); oppure trascina i 3 componenti NPC_VecchiettaBehaviour, NPC_ManagerBehaviour, NPC_TerzoBehaviour.

3. **UI – 3 pulsanti (Ascolta, Offri, Stai)**
   - Sotto **Canvas** (o Panel): crea 3 pulsanti (tasto destro → UI → Button). Rinomina: **Btn_Ascolta**, **Btn_Offri**, **Btn_Stai**.
   - Per ogni pulsante: nell’Inspector → **Button** → **On Click ()** → trascina il GameObject **InteractionManager** (Sistema/InteractionManager) → nella lista delle funzioni scegli **InteractionManager** → **OnListen** (o **OnOffer** / **OnStay** per gli altri due).

4. **Selezione NPC (click)**
   - Aggiungi lo script **NPCSelectionInput** alla **Main Camera** (o al Player): Add Component → `NPCSelectionInput`. Lo script fa raycast al click e chiama `InteractionManager.SetSelectedNpc()` sull'NPC colpito.
   - **Ogni NPC** (NPC_Vecchietta, NPC_Manager, NPC_Terzo) deve avere un **Collider** (es. Box Collider) sul GameObject principale o su un figlio, altrimenti il raycast non li colpisce. Se usi Quad come placeholder, il Quad ha già un collider; altrimenti aggiungi un Box Collider al root dell'NPC.
   - Opzionale: in NPCSelectionInput puoi impostare **Npc Layers** per limitare il raycast solo agli NPC (assegna un layer agli NPC e selezionalo nell'Inspector).
   - *(Obsoleto:* assegna manualmente l’NPC in un test (es. uno script di prova che fa `InteractionManager.SetSelectedNpc(npcVecchietta.transform)` in Start).
   - Per la versione finale: aggiungi un **raycast** (o **Physics2D.OverlapPoint**) al click del mouse; se colpisce un NPC, chiama `InteractionManager.SetSelectedNpc(hit.transform)` e mostra i 3 pulsanti.

5. **Test**
   - Play: seleziona un NPC (o assegnalo in codice), clicca **Stai** sulla Vecchietta (e resta vicino alla panchina) → dopo qualche secondo in console: “Puzzle risolto!”. Stessa cosa per Manager (**Ascolta**) e Terzo (**Offri** o **Stai** vicino a Biglietteria). Quando tutti e 3 sono risolti, WinCondition stampa “Livello completato!”.

---

## 6. COSA NON FARE (per restare minimal)

- Non implementare inventario complesso: “Offri telefono” può essere un singolo pulsante.
- Non fare movimento 3D complesso: “Stai” = player resta fermo X secondi vicino alla panchina.
- Non animare tutto subito: placeholder testi in console vanno bene per test puzzle.
- Non tracciare il tabellone in tempo reale: può essere un testo fisso che “cambia raramente” (anche una sola volta a metà livello).

Quando hai la struttura, i 3 NPC con script e TimeController + InteractionManager, il gioco è già giocabile end-to-end; poi aggiungi arte e feedback.
