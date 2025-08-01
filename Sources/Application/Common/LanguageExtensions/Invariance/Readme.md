# General

The guard is an possibility to force invariants. As C# doesn't have much of native support, the usual approach would be:

```csharp
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }
```

This works, but has some drawbacks:

- No general single point of code flow, therefore f.e. just changing the type doesn't work
- Code duplication: the propert is checked as well as the name via nameof
- Four lines of code for not much of value

The guard class has therefore the following benefits:

- One-liner
- Central point for every check, making stuff like error translations and debugging easier
- Easily extensible for new pattern checks, as the concept revolves around a simple boolean check of a property

The drawback is:
- As we're using Expresions and reflection, the compiler can't inline code. Therefore, there is a small performance hit