public static class NameGenerator
{
    private static string[] firstName = {
    "Zippy", "Bubbles", "Sparky", "Gadget", "Whiskers", "Pogo", "Boomer", "Blasty", "Fizzy", "Squiggle"
};

    private static string[] lastName = {
    "McGee", "O'Wobble", "Thunderpants", "von Fizzlebang", "Widgetworth", "Snickerdoodle", "Bumblethorpe", "Gigglebottom", "Doodle", "Quackenbush"
};

    public static string GetName()
    {
        Random random = new Random();
        string name = string.Empty;
        name += firstName[random.Next(firstName.Length)] + " " + lastName[random.Next(lastName.Length)];

        return name;
    }
}