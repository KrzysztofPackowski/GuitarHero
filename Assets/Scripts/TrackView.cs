using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Beats
{
    [RequireComponent (typeof(VerticalLayoutGroup))]
    [RequireComponent (typeof(ContentSizeFitter))]
    [RequireComponent (typeof(RectTransform))]
    public class TrackView : MonoBehaviour
    {
        [SerializeField] Track _track;

        [SerializeField] RectTransform _left;
        [SerializeField] RectTransform _right;
        [SerializeField] RectTransform _up;
        [SerializeField] RectTransform _down;

        [SerializeField] RectTransform _empty;

        RectTransform _rTransform;

        Vector2 _position;

        public float position {
            get {
                return _position.y;
            }

            set {
                if (value != _position.y)
                {
                    _position.y = value;
                    _rTransform.anchoredPosition = _position;
                }
            }
        }


        public void Init (Track track)
        {
            _rTransform = (RectTransform)transform;
            _position = _rTransform.anchoredPosition;

            foreach (int b in track.beats) {
                GameObject g;
                switch (b) {
                case 0:
                    g = _left.gameObject;
                    break;

                case 1:
                    g = _down.gameObject;
                    break;

                case 2:
                    g = _up.gameObject;
                    break;

                case 3:
                    g = _right.gameObject;
                    break;

                default:
                    g = _empty.gameObject;
                    break;
                }

                GameObject t = GameObject.Instantiate (g, transform);
                t.transform.SetAsFirstSibling ();
            }
        }

        void Start ()
        {
            Init (_track);
        }

        void Update()
        {
            position -= Time.deltaTime * 200f;
        }
    }
}
