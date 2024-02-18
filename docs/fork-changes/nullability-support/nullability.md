---
uid: ForkChanges.Nullability
title: Support for Nullability
---

This fork supports nullable reference types, resulting in less errors for you!

> ![NOTE]
> Although I believe to have fully covered the library with nullable reference type support, there still may be some unforseen issues.
> If you find one, feel free to make a pull request/issue on the github repository.

For example:
```cs
// IGuild.cs
    ...

    // this method is now marked as returning a nullable IGuildUser.
    Task<IGuildUser?> GetUserAsync(ulong id, CacheMode mode = CacheMode.AllowDownload, RequestOptions? options = null);
```