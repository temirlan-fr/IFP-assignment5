var draft = new Draft(1000m);

var validated = Checkout.Validate(draft);

var priced = Checkout.ApplyDiscount(validated, 0.10m);

var receipt = Checkout.Charge(priced);

Console.WriteLine($"Transaction ID: {receipt.TransactionId}");
Console.WriteLine($"Charged: {receipt.Amount:F2}");


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
            validated.Amount - validated.Amount * discountPercent;

        return new Priced(discountedAmount);
    }

    public static Receipt Charge(Priced priced)
    {
        var transactionId = Guid.NewGuid().ToString();

        return new Receipt(
            transactionId,
            priced.Amount);
    }
}