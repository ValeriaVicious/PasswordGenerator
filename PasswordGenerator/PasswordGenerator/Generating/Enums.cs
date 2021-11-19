using System;

namespace PasswordGenerator.Generating
{
    [Flags]
    public enum PasswordGenerationAlphabet : uint
    {
        None =                       0b00000000000000000000000000000000,
        LowerCaseCharacters =        0b00000000000000000000000000000001,
        UpperCaseCharacters =        0b00000000000000000000000000000010,
        Digits =                     0b00000000000000000000000000000100,
        SpecialCharacters =          0b00000000000000000000000000001000,
        Default = LowerCaseCharacters
                  | UpperCaseCharacters
                  | Digits,
        All = Default
              | SpecialCharacters
    }
}
