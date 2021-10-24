using System;
using System.Collections.Generic;

namespace Utils {
    public class Gather {
        public Gather(IReadOnlyCollection<Action<Action>> asyncFuncs, Action onDone) {
            var funcsToGather = asyncFuncs.Count;
            foreach (var asyncFunc in asyncFuncs) {
                asyncFunc(() => {
                    if (--funcsToGather == 0)
                        onDone();
                });
            }
        }
    }
}
