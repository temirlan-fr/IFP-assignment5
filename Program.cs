var draft = new Draft(1000m);
var validated = Checkout.Validate(draft);
var priced = Checkout.ApplyDiscount(validated, 0.10m);


// Real gateway
Console.WriteLine("Real gateway:");

var realReceipt = Checkout.Charge(
    priced,
    RealGateway);

Console.WriteLine(
    $"Transaction ID: {realReceipt.TransactionId}");

Console.WriteLine(
    $"Charged: {realReceipt.Amount:F2}");


// Fake gateway
Console.WriteLine();
Console.WriteLine("Fake gateway:");

var fakeGateway = new FakeGateway();

var fakeReceipt = Checkout.Charge(
    priced,
    fakeGateway.Charge);

Console.WriteLine(
    $"Transaction ID: {fakeReceipt.TransactionId}");

Console.WriteLine(
    $"Charged: {fakeReceipt.Amount:F2}");

Console.WriteLine(
    $"Gateway calls: {fakeGateway.Calls}");


static string RealGateway(decimal amount)
{
    Console.WriteLine(
        $"Real gateway charged {amount:F2}");

    return $"real-{Guid.NewGuid()}";
}


public sealed record Draft(decimal Amount);

public sealed record Validated(decimal Amount);

public sealed record Priced(decimal Amount);

public sealed record Receipt(
    string TransactionId,
    decimal Amount
);


public static class Checkout
{
    public static Validated Validate(Draft draft)
    {
        if (draft.Amount <= 0)
        {
            throw new InvalidOperationException(
                "Amount must be greater than zero.");
        }

        return new Validated(draft.Amount);
    }

    public static Priced ApplyDiscount(
        Validated validated,
        decimal discountPercent)
    {
        var discountedAmount =
            validated.Amount -
            validated.Amount * discountPercent;

        return new Priced(discountedAmount);
    }

    public static Receipt Charge(
        Priced priced,
        Func<decimal, string> gateway)
    {
        var transactionId = gateway(priced.Amount);

        return new Receipt(
            transactionId,
            priced.Amount);
    }
}


public sealed class FakeGateway
{
    public int Calls { get; private set; }

    public string Charge(decimal amount)
    {
        Calls++;

        return $"fake-transaction-{Calls}";
    }
}