﻿namespace ReflectionSample.Tests.Models
{
    public class FPrivate 
    { 
        int i1, i2, i3, i4, i5; 
        public FPrivate Get() => new FPrivate() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 }; 
    }
}
