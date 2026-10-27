Console.WriteLine("1. Discount applied twice");

var discountTwice = new Checkout();

discountTwice.Start(1000m);
discountTwice.Validate();
discountTwice.ApplyDiscount(0.10m);
discountTwice.ApplyDiscount(0.10m);
discountTwice.Charge();


Console.WriteLine();
Console.WriteLine("2. Discount forgotten");

var discountForgotten = new Checkout();

discountForgotten.Start(1000m);
discountForgotten.Validate();
discountForgotten.Charge();


Console.WriteLine();
Console.WriteLine("3. Charge called twice");

var chargedTwice = new Checkout();

chargedTwice.Start(1000m);
chargedTwice.Validate();
chargedTwice.ApplyDiscount(0.10m);
chargedTwice.Charge();
chargedTwice.Charge();


Console.WriteLine();
Console.WriteLine("4. Charge before Validate");

var notValidated = new Checkout();

notValidated.Start(1000m);

try
{
    notValidated.Charge();
}
catch (InvalidOperationException exception)
{
    Console.WriteLine($"Runtime error: {exception.Message}");
}


public class Checkout
{
    private decimal _total;
    private bool _validated;

    public void Start(decimal amount)
    {
        _total = amount;
    }

    public void Validate()
    {
        _validated = _total > 0;
    }

    public void ApplyDiscount(decimal percent)
    {
        _total -= _total * percent;
    }

    public void Charge()
    {
        if (!_validated)
        {
            throw new InvalidOperationException("Not validated.");
        }

        Console.WriteLine($"Charged {_total:F2}");
    }
}