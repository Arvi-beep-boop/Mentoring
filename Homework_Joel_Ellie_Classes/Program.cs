public abstract class Character
{
    public Vector2 Position = new Vector2(0.0f, 0.0f);
    public abstract string GetName();
}

public class Vector2
{
    public float X;
    public float Y;

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public float DistanceTo(Vector2 vector)
    { 
        return  (float)Math.Sqrt(Math.Pow(X - vector.X, 2) + Math.Pow(Y - vector.Y, 2)); 
    }
}

class BarkSystem
{
    List<DialogueScene> Barks;
    List<Character> Characters;
    public BarkSystem(List<DialogueScene> barks, List<Character> characters)
    {
        Barks = barks;
        Characters = characters;
    }   
    public Character FindNearbyCharacter(Character character)
    {
        float min_distance = float.MaxValue; // not an infinity but it will work
        Character min_character = null;
        foreach(Character obj in Characters)
        {
            if (obj != character)
            {
                float distance = character.Position.DistanceTo(obj.Position);
                if(distance < min_distance)
                {
                    min_distance = distance;
                    min_character = obj;
                }
            }
        }
        return min_character;
    }
    public void PlayBark(Character character)
    {

        Character NearbyCharacter = FindNearbyCharacter(character);
        if (NearbyCharacter != null)
        {
            Random r = new Random();
            int rInt = r.Next(0, Barks.Count);
            List<Character> characters = new List<Character>();
            characters.Add(character);
            characters.Add(NearbyCharacter);
            Barks[rInt].Play(characters);
        }
    }
}
class Joel : Character
{
    public override string GetName()
    {
        return "Joel";
    }
}

class Ellie : Character
{
    public override string GetName()
    {
        return "Ellie";
    }
}
class Tommy : Character
{
    public override string GetName()
    {
        return "Tommy";
    }
}
class DialogueLine
{
    public int SpeakerIndex;
    public string Text;

    public DialogueLine(int speakerIndex, string text)
    {
        Text = text;
        SpeakerIndex = speakerIndex;
    }

}

class DialogueScene
{
    
    private List<DialogueLine> Lines;

    public DialogueScene(List<DialogueLine> lines)
    {
        Lines = lines;
    }
    public void Play(List<Character> characters)
    {
        DialogueLine SingleLine;
        for(int i = 0; i < Lines.Count; i++)
        {
            SingleLine = Lines[i];
            Console.Write(characters[SingleLine.SpeakerIndex].GetName());
            Console.Write(": ");
            Console.Write(SingleLine.Text);
            Console.WriteLine();
        }
    } 

}
class Progam
{
    static void Main()
    {
        List<DialogueLine> WoofLines1 = new List<DialogueLine>();
        WoofLines1.Add(new DialogueLine(0, "I'm out of ammo!"));
        WoofLines1.Add(new DialogueLine(1, "Catch!"));

        List<DialogueLine> WoofLines2 = new List<DialogueLine>();
        WoofLines2.Add(new DialogueLine(0, "Cover me!"));
        WoofLines2.Add(new DialogueLine(1, "On it!"));

        List<DialogueScene> Woof = new List<DialogueScene>();
        Woof.Add(new DialogueScene(WoofLines1));
        Woof.Add(new DialogueScene(WoofLines2));

        List<Character> Characters = new List<Character>();
        Ellie ellie = new Ellie();
        Joel joel = new Joel();
        Tommy tommy = new Tommy();

        Characters.Add(ellie);
        Characters.Add(joel);
        Characters.Add(tommy);

        ellie.Position.X = 2.0f;
        ellie.Position.Y = 0.0f;
        joel.Position.X = 0.0f;
        joel.Position.Y = 3.0f;
        tommy.Position.X = -1.0f;
        tommy.Position.Y = 0.0f;


        BarkSystem barkSystem = new BarkSystem(Woof, Characters);

        //List<DialogueLine> dialogueLines = new List<DialogueLine>();
        //dialogueLines.Add(new DialogueLine(0, "You gotta see this!"));
        //dialogueLines.Add(new DialogueLine(1, "What is it?"));
        //dialogueLines.Add(new DialogueLine(0, "Are you kidding me?!"));
        //dialogueLines.Add(new DialogueLine(0, "C'mon, hurry up!"));
        //dialogueLines.Add(new DialogueLine(0, "You see this?"));
        //dialogueLines.Add(new DialogueLine(0, "Shh, don't scare it!"));
        //dialogueLines.Add(new DialogueLine(1, "I won't, I won't..."));
        //dialogueLines.Add(new DialogueLine(0, "What are you doin'?"));
        //dialogueLines.Add(new DialogueLine(1, "It's alright, come here, come here"));
        //dialogueLines.Add(new DialogueLine(1, "Hurry up!"));
        //dialogueLines.Add(new DialogueLine(1, "C'mon."));
        //dialogueLines.Add(new DialogueLine(0, "Hi there."));
        //dialogueLines.Add(new DialogueLine(0, "Huh. So fuckin' cool."));
        //DialogueScene scene = new DialogueScene(dialogueLines);
        //scene.Play(Characters);

        barkSystem.PlayBark(joel);
        barkSystem.PlayBark(tommy);
    }
}
