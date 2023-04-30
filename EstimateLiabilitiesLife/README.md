# Pension

### Description

The module contains a type named cashflow which has three fields: month, t, and benefit. It also contains another type
named cashflows which is a list of cashflow types.

The reserve function takes three arguments: valuedate of type DateTime, contract of type Contract, and discount of type
float list.

The code also defines two private functions named benefits and projectCashflows. The benefits function takes four
arguments: index of type int list, benefit of type float, z_month of type int, and end_month of type int. It returns a
list of floats that are either equal to benefit or 0 depending on whether the corresponding element in index is between
z_month and end_month.

The projectCashflows function takes two arguments: valuedate of type DateTime and insurance contract of type Contract.
It returns a list of cashflows that are calculated based on the values in the contract.