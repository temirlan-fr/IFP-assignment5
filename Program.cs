var draft = new Draft(1000m);
var validated = Checkout.Validate(draft);
var priced = Checkout.ApplyDiscount(validated, 0.10m);

var receipt = Checkout.Charge(
    priced,
    RealGateway);

Console.WriteLine($"Transaction ID: {receipt.TransactionId}");
Console.WriteLine($"Charged: {receipt.Amount:F2}");


// Task 1 wrong sequence 1:
// Discount applied twice.
//
// var pricedAgain = Checkout.ApplyDiscount(priced, 0.10m);
//
// Compiler error:
// CS1503: cannot convert from 'Priced' to 'Validated'.
// The second discount is rejected by the compiler.


// Task 1 wrong sequence 2:
// Discount forgotten.
//
// var invalidReceipt = Checkout.Charge(
//     validated,
//     RealGateway);
//
// Compiler error:
// CS1503: cannot convert from 'Validated' to 'Priced'.
// Charge requires a Priced value.


// Task 1 wrong sequence 3:
// Charge called twice.
//
// var chargedAgain = Checkout.Charge(
//     receipt,
//     RealGateway);
//
// Compiler error:
// CS1503: cannot convert from 'Receipt' to 'Priced'.
// A Receipt cannot be charged again.


// Task 1 wrong sequence 4:
// Charge before Validate.
//
// var invalidReceipt = Checkout.Charge(
//     draft,
//     RealGateway);
//
// Compiler error:
// CS1503: cannot convert from 'Draft' to 'Priced'.
// Draft must first pass through Validate and ApplyDiscount.


static string RealGateway(decimal amount)
{
    Console.WriteLine($"Gateway charged {amount:F2}");

    return $"transaction-{Guid.NewGuid()}";
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