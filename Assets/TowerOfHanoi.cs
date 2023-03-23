using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerOfHanoi : MonoBehaviour {
    [SerializeField] private GameObject TOWER_1;
    [SerializeField] private GameObject TOWER_2;
    [SerializeField] private GameObject TOWER_3;

    [SerializeField] private GameObject discPrefab;

    [SerializeField] private int discsAmount = 3;

    [SerializeField] private Color[] colors;
    private GameObject[] discs;

    private void Start() {
        discs = new GameObject[discsAmount + 1];
        InitDiscs();

        StartCoroutine(MoveDiscs(discsAmount, TOWER_1, TOWER_3, TOWER_2, .5f));
    }



    private void InitDiscs() {
        Vector3 discPosition = new Vector3(TOWER_1.transform.position.x, -2f, 0);

        // save scale from prefab
        Vector3 oldScale = discPrefab.transform.localScale;

        GameObject newDisc;
        for (int i = 0; i < discsAmount; i++) {
            newDisc = Instantiate(discPrefab, discPosition, Quaternion.identity);
            newDisc.GetComponent<SpriteRenderer>().color = colors[i];
            discs[i] = newDisc;

            discPosition = new Vector3(discPosition.x, discPosition.y + 1, 0f);
            discPrefab.transform.localScale = new Vector3(newDisc.transform.localScale.x - .8f, newDisc.transform.localScale.y, 0);
        }

        // change scale back to original state
        discPrefab.transform.localScale = oldScale;
        Array.Reverse(discs);
    }

    private IEnumerator MoveDiscs(int n, GameObject FROM, GameObject TO, GameObject AUX, float duration) {

        if (n == 0) yield break;

        yield return MoveDiscs(n - 1, FROM, AUX, TO, duration);

        float time = 0;
        Vector3 startPosition = discs[n].transform.position;
        Vector3 targetPosition = TO.transform.position + new Vector3(0, 3f, 0);

        discs[n].GetComponent<Collider2D>().enabled = false;
        while (time < duration) {
            discs[n].transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        discs[n].transform.position = targetPosition;
        discs[n].GetComponent<Collider2D>().enabled = true;


        Debug.LogFormat("Move {0} from {1} to {2}", n, FROM, TO);
        yield return new WaitForSeconds(duration);


        yield return MoveDiscs(n - 1, AUX, TO, FROM, duration);
    }


}
