using System;

namespace bib_Vererbung_003
{
    public class Luftfahrzeug
    {
        public string Hersteller { get; set; }
        public int Baujahr { get; set; }

        public virtual void Starten()
        {
            Console.WriteLine("Das Luftfahrzeug startet.");
        }

        public void Ausgaben(Luftfahrzeug lfzg)
        {
            if (lfzg != null)
            {
                // Der is-operator
                if (lfzg is Flugzeug)
                    Console.WriteLine("Spannweite: ", ((Flugzeug)lfzg).Spannweite);
                else if (lfzg is Hubschrauber)
                    Console.WriteLine("Rotor: ", ((Hubschrauber)lfzg).Rotor);
                //else if (lfzg is Luftfahrzeug)
                //    Console.WriteLine("Es handelt sich allgemein um ein Luftfahrzeug");
                else
                    Console.WriteLine("Unbekannter Typ.");
            }
        }
    }

    public class Flugzeug : Luftfahrzeug
    {
        public double Spannweite { get; set; }

        // 1.) 2019-12-13
        // Die Methode »ToString()« der Klasse »Object« überschreiben
        public override string ToString()
        {
            return "Overridden";
        }

        // 2.a) 2019-12-13
        // Versiegelte Methode können nicht weiter abgeleitet werden
        public override void Starten()
//            public sealed override void Starten()
        {
            Console.WriteLine("Das Flugzeug startet.");
        }
    }

    // 2.b) 2019-12-13
    // Versiegelte Methode können nicht weiter abgeleitet werden
    public class SegelFlugzeug : Flugzeug
    {
        public override void Starten()
        {
            Console.WriteLine("Das SegelFlugzeug startet.");
        }
    }

    public class Hubschrauber : Luftfahrzeug
    {
        public double Rotor { get; set; }
    }




    // 3.a) 2019-12-13
    // Interface - Zwischengesicht
    // Statt class schreibt man interface
    // Konvention: Name des interface vorgestelltes großes I

    // Von einem Interface kann kein Objekt erzeugt werden
    // Frage: Können von abstrkaten Klassen Objekte erzeugt werden ?
    public interface IKopieren
    {
        // Attribute enthalten keine Modifizierer
//        string beschriftung;
        string beschriftung { get; set; }

        // Methoden werden wie bei abstrakten Klassen ohne Methoden-Rumpf definiert,
        // d.h. ohne Anweisungsblock
        // Interfaces enthalten nur abstrakte Definitionen
        // Modifizierer: pulic oder internal
        // Methoden haben keinen Zugriffsmodifizierer (z.B. public)
        // Frage: Wo ist dann der Unterschied zu abstrakten Methoden ?
        void Kopiere();
    }

    // 3.b) 2019-12-13
    // Interface wird implementiert
    // Ein Interface ist wie ein Vertrag, mit Konsequenzen (s.u.):
    class CDokument : IKopieren
    {
        // Alle Member müssen nun übernommen werden
        public string beschriftung;
        string IKopieren.beschriftung
        {
            get { return beschriftung; }
            set { beschriftung = value; }
        }

        // Auch alle Methoden müssen übernommen werden
        // Frage: kommt uns das irgendwie bekannt vor ?
        public void Kopiere()
        {
            Console.WriteLine("Das Dokument '" + beschriftung + "' wird kopiert.");
        }
    }


    class CDokument2 : Flugzeug, IKopieren, IDisposable
    {
        // Alle Member müssen nun übernommen werden
        private string beschriftung;
        string IKopieren.beschriftung
        {
            get { return beschriftung; }
            set { beschriftung = value; }
        }


        // Auch alle Methoden müssen übernommen werden
        // Frage: kommt uns das irgendwie bekannt vor ?
        // Die Methode muss public, abstract oder virtual sein !
        // Bemerkung: wir arbeiten hier nur mit public.
        public void Kopiere()
        {
            Console.WriteLine("Das Dokument '" + beschriftung + "' wird kopiert.");
        }

        public void Dispose()
        {

        }
    }

    class Maschine : IKopieren
    {
        // Einen Maschine kopieren im Sinne von Nachbauen
        public string beschriftung;
        string IKopieren.beschriftung
        {
            get { return beschriftung; }
            set { beschriftung = value; }
        }

        // Auch alle Methoden müssen übernommen werden
        // Frage: kommt uns das irgendwie bekannt vor ?
        public void Kopiere()
        {
            Console.WriteLine("Die Maschine wird kopiert.");
        }
    }

// ------------------------------------------
    class Program
    {
        static void Main(string[] args)
        {
            Flugzeug flugi = new Flugzeug();
            flugi.Starten();
            Console.WriteLine(flugi.Spannweite);

            Hubschrauber hubidubi = new Hubschrauber();
            hubidubi.Starten();
            Console.WriteLine();


            // Die implizite Typumwandlung
            Flugzeug flg = new Flugzeug();
            // Basisklassenreferenz = Subklassenreferenz
            Luftfahrzeug lfzg = flg;
            // lfzg und flg referenzieren nun den gleichen Speicherbereich.
            // Allerding können die spez. Member von Flugzeug in lfzg nicht verwendet werden !
            // lfzg.Spannweite = 3000;  // ERROR: Datenverlust !
            flg.Spannweite = 4000;      // Nach wie vor OK.


            // Die Methode Ausgaben erwartet vom Aufrufer die Referenz auf ein Luftfahrzeug.
            lfzg.Ausgaben(lfzg);

            // Dies kann auch eine Referenz von einer abgeleiteten Klasse sein !
            lfzg.Ausgaben(flg);
            lfzg.Ausgaben(hubidubi);
            Console.WriteLine();


            // Polymorphie - mit override bzw. abstrkaten Methoden
            Luftfahrzeug[] arr = new Luftfahrzeug[4];
            arr[0] = new Flugzeug();
            arr[1] = new Hubschrauber();
            arr[2] = new Hubschrauber();
            arr[3] = new Flugzeug();
            foreach (Luftfahrzeug temp in arr)
            {
                temp.Starten();
            }




            // 1.) 2019-12-13
            // Überschreiben der virtuellen Methode ToString()
            Console.WriteLine(arr[0].ToString());



            // 2.) 2019-12-13
            // Versiegelte Methode können nicht weiter abgeleitet werden
            SegelFlugzeug segel = new SegelFlugzeug();
            segel.Starten();
            Console.WriteLine();



            // 3.) 2019-12-13
            CDokument doku = new CDokument();
            doku.beschriftung = "Dies ist eine Beschriftung";

            // Die Methode die von einem Interface implementiert wurde, ist immer polymorph (!),
            // (solange sie nicht mit new verdeckt wird) !
            // Kopiere() ist polymorph. Frage: was bedeutet das ?
            doku.Kopiere();
            Console.WriteLine();

            // Regeln zum Implemtieren mit Interfaces:
            // 1.) Eine Klasse kann mehrere Schnittstellen implementiert werden, durch Komma getrennt.
            // Bsp: class CDokument : IKopieren, IDisposable { }
            // 2.) Eine Klasse kann abgeleitet UND implemtiert werden.
            // ACHTUNG: dabei muss als allererstes die Basisklasse nach dem Doppelpunkt aufgeführt werden !
            // class CDokument2 : Dokument, IKopieren, IDisposable { }
            // 3.) Schnittstellen dürfen nach der Veröffentlichung nie wieder verändert werden?
            // Fragen: Warum? Was ist dann zu tun?
            // 4.) Schnittstellen können selbst auch wieder Schnittstellen implementieren.
            // Es entsteht so EINE neue Schnittstelle.


            // 4.) 2019-12-13
            IKopieren idoku = doku;
            idoku.Kopiere();
            // Frage: kommt Ihnen das bekannt vor?
            // => Basisklassenreferenz = Subklassennreferenz
            // Das bedeutet: Eine Schnittstelle kann genauso behandelt werden,
            // als handele es sich um eine Basisklasse,
            // mit erheblichen Konsequenzen !


            // 5.) Warum gibt es Schnittstellen ?
            // Vergleichen Sie oben stehenden Code mit dem schon bekannten Code:
            //Flugzeug flg = new Flugzeug();
            //Luftfahrzeug lfzg = flg;

            // Was passiert, wenn eine völlig andere Klasse, die nichts mit Dokumenten zu tun hat,
            // IKopieren implementiert ? Z.B. eine Maschine ?
            IKopieren[] ikop = new IKopieren[3];
            ikop[0] = new CDokument();
            ikop[1] = new CDokument2();
            ikop[2] = new Maschine();
            foreach (IKopieren item in ikop)
            {
                item.Kopiere();
            }
            Console.WriteLine();
            // Das mit der Maschine funktioniert also, weil alle dieselbe "Basis" haben.
            // Ergebnis:
            // Mit Schnittstellen lassen sich Klassen schaffen, die sich wie Ableitungen,
            // also wie bei einer Vererbung verhalten, indem die Objekte von Klassen,
            // die die Interfaces implementieren, auch Objekte von Interfaces sind, sozusagen.
            // Vergleiche: Objekte von Subklassen sind immer auch Objekte von Basisklassen
            // z.B. ein Haus ist ein Gebäude.





            Console.ReadLine();
        }
    }
}
