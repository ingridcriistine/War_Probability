using System.Runtime;

Random rand = new Random();
bool game = true;
int vitoriasAtaque = 0;
int vitoriasDefesa = 0;

void War(Soldado atacantes, Soldado defensores) {

    List<int> dadosAtaque = [];
    List<int> dadosDefesa = [];

    for (int i = 0; i < (atacantes.qtdSoldados < 4 ? atacantes.qtdSoldados - 1 : 3); i++)
        dadosAtaque.Add(rand.Next(6) + 1);

    for (int i = 0; i < (defensores.qtdSoldados < 4 ? defensores.qtdSoldados : 3); i++)
        dadosDefesa.Add(rand.Next(6) + 1);

    dadosAtaque.Sort();
    dadosAtaque.Reverse();
    dadosDefesa.Sort();
    dadosDefesa.Reverse();

    // Console.WriteLine("Ataque");
    // for (int i = 0; i < dadosAtaque.Count(); i++)
    // {
    //     Console.WriteLine(dadosAtaque[i]);
    // }

    // Console.WriteLine("Defesa");
    // for (int i = 0; i < dadosDefesa.Count(); i++)
    // {
    //     Console.WriteLine(dadosDefesa[i]);
    // }

    for (int i = 0; i < (dadosAtaque.Count() < dadosDefesa.Count() ? dadosAtaque.Count() : dadosDefesa.Count()); i++)
    {
        if(dadosAtaque[i] > dadosDefesa[i])
            defensores.qtdSoldados -= 1;
        else
            atacantes.qtdSoldados -= 1;
    }
}

for (int i = 0; i < 10_000; i++)
{    
    Soldado atacantes = new(1000);
    Soldado defensores = new(500);
    game = true;

    while(game) {
        War(atacantes, defensores);
            
        if(atacantes.qtdSoldados <= 1) {
            Console.WriteLine("\nA defesa venceu!!");
            vitoriasDefesa += 1;
        }

        if(defensores.qtdSoldados == 0) {
            Console.WriteLine("\nO ataque venceu!!");
            vitoriasAtaque += 1;
        }

        if(atacantes.qtdSoldados <= 1 || defensores.qtdSoldados == 0) 
            game = false;
    }
}

// Console.WriteLine($"Nº Atacantes: {atacantes.qtdSoldados}");
// Console.WriteLine($"Nº Defensores: {defensores.qtdSoldados}");
Console.WriteLine($"O ataque venceu {vitoriasAtaque * 100 / (vitoriasAtaque+vitoriasDefesa)}% das vezes."); 