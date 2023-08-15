using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public  class ObjectMerger : MonoBehaviour, IMergeAble
{
    private AudioSource _audioSource;
    private ParticleSystem _mergeParticleSystem;
    private GridGenerator _gridGenerator;
    [SerializeField] private AudioClip _audioClip;
    
    void Awake()
    {
        _gridGenerator = GetComponent<GridGenerator>();
        _audioSource = GetComponent<AudioSource>();
        _mergeParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        _gridGenerator.MergeAction += Merge;
    }

    private void OnDisable()
    {
        _gridGenerator.MergeAction -= Merge;
    }

    public void Merge(List<Item> itemList, string receivedName, Vector3 mergePosition)
    {
        //Debug.Log("entered merge method");
        _audioSource.Play();
        //Debug.Log("merge Position: " + mergePosition + " in merge method.");
        foreach (var item in itemList)
        {
            //Debug.Log(fruitDictionary[fruitItem.fruitName].FruitScriptObjects.Count + "   count");
            if (item.fruitName == receivedName)
            { 
                //Debug.Log("Contains Key. " + "Received Name: " + ReceivedName);
                if (itemList.Count >= 3 && mergePosition != Vector3.zero)
                {
                    Debug.Log("Merging start");
                    //Debug.Log("Count = 3");
                    item.transform.DOMoveX(mergePosition.x, 0.75f).SetEase(Ease.InExpo).OnComplete((() =>
                    {
                        _mergeParticleSystem.transform.position = mergePosition; // setting the effect where the merge is happening
                        _audioSource.PlayOneShot(_audioClip);
                        _mergeParticleSystem.Play();
                        item.transform.DOMoveY(2f, 1f).SetEase(Ease.Linear).WaitForCompletion();
                        item.transform.DOScale(0.75f, 0.75f).SetEase(Ease.Linear).OnComplete((() =>
                        {
                             item.transform.DOScale(0f, 0.1f).SetEase(Ease.OutBounce).OnComplete((() =>
                             { 
                                 Transform fruitGameObject = item.transform;
                                 //Debug.Log("Destroying");
                                 _gridGenerator.ItemDictionary.Remove(item.fruitName);
                                Destroy(fruitGameObject.gameObject, 0.1f);
                                _gridGenerator.ItemDictionary[receivedName].Count = 0;
                             }));
                        }));    
                    }));
                }
                else
                {
                    //Debug.Log("not Merging");
                    //Debug.Log("Count is not 3, so exiting");
                }
            }
        }
    }
}