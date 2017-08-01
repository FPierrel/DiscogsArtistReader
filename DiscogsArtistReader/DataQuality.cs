using System;
using System.Collections.Generic;
using System.Text;

namespace DiscogsArtistReader
{
    public enum DataQuality
    {
        Correct,
        CompleteAndCorrect,
        NeedsVote,
        NeedsMinorChanges,
        NeedsMajorChanges,
        EntirelyIncorrect
    }
}
