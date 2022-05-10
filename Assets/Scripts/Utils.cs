using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

    void Start() {

    }


    void Update() {

    }

    public static T RandomSelect<T>(List<T> list) {
        var index = Random.Range(0, list.Count);
        return list[index];
    }

}
