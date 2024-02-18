---
uid: ForkChanges.NumericValue
title: Numeric Value Type
---

`NumericValue` has been added as a new type for `Min` and `Max` values for application commands. They can represent decimal numbers, and integer numbers.

This type was added due to `double`s not being able to completely store a `long` without some precision lost. As a result, this type was made that stores both a `double` and a `long`, resulting in more precision when serializing/deserializing json.