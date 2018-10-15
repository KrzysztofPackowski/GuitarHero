using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Beats
{
    public class GameplayController : MonoBehaviour
    {

        [Header ("Input")]
        [SerializeField] KeyCode _left;
        [SerializeField] KeyCode _down;
        [SerializeField] KeyCode _up;
        [SerializeField] KeyCode _right;



        [Header ("Track")]
        [Tooltip ("Beats Track to play")]
        [SerializeField] Track _track;

        /// <summary>
        /// The current Track.
        /// </summary>
        public Track track { get { return _track; } }

        public float beatsPerSecond { get; private set; }

        public float secondsPerBeat { get; private set; }

        bool _played;
        bool _completed;

        TrackView _trackView;

        WaitForSeconds waitAndStop;

        static GameplayController _instance;

        public static GameplayController Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (GameplayController)GameObject.FindObjectOfType (typeof(GameplayController));
                }
                return _instance;
            }
            set 
            {
                _instance = value;
            }
        }


        #region MonoBehaviour Methods

        void Awake ()
        {
            _instance = this;
            _trackView = FindObjectOfType<TrackView> ();
            if (!_trackView)
                Debug.Log ("No TrackView found in current scene");

            beatsPerSecond = track.bpm / 60f;
            secondsPerBeat = 60f / track.bpm;

            waitAndStop = new WaitForSeconds (secondsPerBeat * 2);
        }

        void OnDestroy()
        {
            _instance = null;
        }

        void Start ()
        {
            InvokeRepeating ("NextBeat", 0f, secondsPerBeat);
        }

        void Update ()
        {
            if (_played || _completed) {
                return;
            }

            if (Input.GetKeyDown (_left))
                PlayBeat (0);
            if (Input.GetKeyDown (_down))
                PlayBeat (1);
            if (Input.GetKeyDown (_up))
                PlayBeat (2);
            if (Input.GetKeyDown (_right))
                PlayBeat (3);
        }

        #endregion

        #region Gameplay

        private int _current;

        public int current {
            get {
                return _current;
            }
            set {
                _current = value;

                if (_current == _track.beats.Count)
                {
                    CancelInvoke ("NextBeat");
                    _completed = true;
                    StartCoroutine (WaitAndStop());
                }
            }
        }

        void PlayBeat (int input)
        {
            //Debug.Log (input);
            _played = true;

            if (_track.beats [current] == -1)
            {
                //Debug.Log (string.Format ("{0} played untimely", input));
            }
            else if (_track.beats [current] == input)
            {
               // Debug.Log (string.Format ("{0} played right", input));
                _trackView.TriggerBeatView (current, TrackView.Trigger.Right);
            }
            else
            {
               // Debug.Log (string.Format ("{0} played, {1} expected", input, _track.beats [current]));
                _trackView.TriggerBeatView (current, TrackView.Trigger.Wrong);
            }
        }

        void NextBeat ()
        {
            //Debug.Log ("Tick");
            if (!_played && _track.beats [current] != -1) {
                //Debug.Log (string.Format ("{0} missed", _track.beats [current]));
                _trackView.TriggerBeatView (current, TrackView.Trigger.Missed);
            }
            _played = false;
            current++;
        }

        private IEnumerator WaitAndStop()
        {
            yield return waitAndStop;
            enabled = false;
        }

        #endregion
    }
}