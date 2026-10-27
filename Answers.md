WHAT IS TEMPORAL COUPLING? WHY IS THE RUNTIME CHECK INSIDE CHARGE NOT ENOUGH?

Temporal coupling means that operations must be called in a specific order. In the original design, Start, Validate, ApplyDiscount, and Charge had to be called in the correct order, but the compiler did not enforce this order. The runtime check inside Charge is not enough because it only checks whether validation happened. It does not detect a forgotten discount, a discount applied twice, or Charge being called twice.

HOW DO THE STAGE TYPES ENFORCE THE ORDER? NAME ONE MISTAKE IN THIS PROCESS THAT YOUR TYPES STILL CANNOT PREVENT.

The stage types represent different states of the checkout process. Validate accepts a Draft and returns a Validated. ApplyDiscount accepts a Validated and returns a Priced. Charge accepts a Priced and returns a Receipt. The compiler rejects calls that use the wrong stage type.

The types cannot prevent someone from creating a Priced object directly. They also cannot prevent the same Priced object from being charged twice.

WHY DOES PASSING THE GATEWAY AS A DELEGATE MAKE CHARGE EASIER TO TEST? WHICH PARTS OF YOUR DESIGN HAVE NO SIDE EFFECTS?

Passing the gateway as a delegate allows Charge to use different gateway implementations. The real gateway can be used in production, while a fake gateway can be used in tests. The fake gateway does not contact a bank and can count how many times it was called. This makes the tests fast and predictable.

Validate and ApplyDiscount have no side effects. Charge performs the payment through the gateway, which is the only side effect in the design.

WHY IS IT NATURAL HERE THAT EVERY STAGE IS A NEW IMMUTABLE OBJECT INSTEAD OF A MODIFIED ONE? WHAT DOES IT COST, AND WHAT DOES IT ALLOW?

Every stage represents a different valid state of the checkout process. A new immutable object makes the state change clear and prevents previous stages from being modified accidentally.

The cost is that the program creates more objects and may copy some values. The benefits are safer code, fewer hidden state changes, easier testing, and compiler enforced ordering.