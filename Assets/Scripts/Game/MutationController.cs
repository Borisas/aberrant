using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MutationController {


    public MutationId[] GenerateMutationSelection(int count = 3) {

        var activeMutations = CollectPlayerMutations();
        var allMutations = Database.GetInstance().Main.PlayerMutations.Where(x => {

            if (!x.Obtainable) return false;

            var mut = activeMutations.FirstOrDefault(y => y.Id == x.Id);
            if ( mut != null ) {
                if (mut.Level >= x.MaxLevel) return false;
            }

            return true;
        }).ToList();


        if ( allMutations.Count <= 0 ) {
            return new MutationId[0];
        }
        else if ( allMutations.Count <= count ) {
            return allMutations.Select(x=>x.Id).ToArray();
        }

        var r = new MutationId[count];

        for ( int i = 0; i < count; i++ ) {
            r[i] = allMutations.ExtractRandom().Id;
        }

        return r;
    }

    IEnumerable<MutationInstance> CollectPlayerMutations() => Scene.Player.GetStats().GetActiveMutations();
}
