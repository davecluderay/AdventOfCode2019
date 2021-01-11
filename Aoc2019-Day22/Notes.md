# Day 22: Slam Shuffle

## Combining the dealing strategies together
--------------------------------------------

Each deal strategy can be represented as a linear function:

* Deal with increment a:

      f(n) = a * n (mod N)

* Cut b:

      f(n) = n - b (mod N)

* Deal into new stack:

      f(n) = -n - 1 (mod N)

Two linear functions over can be combined (even when over mod N), so:

    f(n) = an + b (mod N)
    g(n) = pn + q (mod N)

can be combined to:

    fg(n) = pan + pb + q (mod N)

## Repeating to create a mega-shuffle
-------------------------------------

Combining the same function k times is a special case:

    1 time:  an   + b            (mod N)
    2 times: aan  + ab  + b      (mod N)
    3 times: aaan + aab + ab + b (mod N)
    ...
    k times: a(^k)n + a(^k-1)b + a(^k-2)b + ... + a(^2)b + b
 
This is itself a linear function:

    f(n) = a'n + b'

where:

   * the increment part is simply:

         a' = a^k

   * the offset part is a [geometric series](https://en.wikipedia.org/wiki/Geometric_progression#Geometric_series), and has a formula:

         b' = b(1 - a^k) / (1 - a)

## Working out the `n` from a known result
------------------------------------------

Given that the mega-shuffle is a linear function over mod N:

    f(n) = an + b (mod N)

Rearranging this to find n:

    n = (v - b) / a (mod N)

Use Euler's formula for modular multiplicative inverse (https://en.wikipedia.org/wiki/Modular_multiplicative_inverse):

    n = (v - b) * a(^N-2) (mod N)