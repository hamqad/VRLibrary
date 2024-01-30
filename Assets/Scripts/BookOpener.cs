using com.guinealion.animatedBook;
using Oculus.Interaction;
using UnityEngine;

namespace Oculus.Interaction.HandGrab
{
    public class BookOpener : MonoBehaviour, ITransformer
    {
        public LightweightBookHelper bookHelper;
        public BoxCollider collider;
        public Rigidbody rb;
        public AudioSource close, open;
        private AudioSource[] audioSources;
        private IGrabbable _grabbable;
        private HandGrabInteractable _grabInteractable;




        private float colliderBaseX, colliderBaseZ, colliderBaseZC; // 0.37 z 0.43x

        public struct Point
        {
            public float X;
            public float Y;

            public Point(float x, float y)
            {
                X = x;
                Y = y;
            }

            // Overload the '*' operator to multiply a point by a scalar
            public static Point operator *(float scalar, Point point)
            {
                return new Point(scalar * point.X, scalar * point.Y);
            }

            // Overload the '+' operator to add two points
            public static Point operator +(Point p1, Point p2)
            {
                return new Point(p1.X + p2.X, p1.Y + p2.Y);
            }
        }

        void Start()
        {
            bookHelper = GetComponent<LightweightBookHelper>();
            collider = GetComponent<BoxCollider>();
            rb = GetComponent<Rigidbody>();

            audioSources = GetComponents<AudioSource>();
            close = audioSources[0];
            open = audioSources[3];
            colliderBaseX = collider.size.x;
            colliderBaseZ = collider.size.z;
            colliderBaseZC = collider.center.z;
        }


        public void Initialize(IGrabbable grabbable)
        {
            _grabbable = grabbable;
            _grabInteractable = grabbable.Transform.GetComponent<HandGrabInteractable>();
        }

        public void BeginTransform()
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        public void UpdateTransform()
        {
            Vector3 hand1Position = _grabbable.GrabPoints[0].position;
            Vector3 hand2Position = _grabbable.GrabPoints[1].position;

            float handsDistance = Vector3.Distance(hand1Position, hand2Position);
            float openAmmount = Mathf.Clamp(handsDistance / 0.5f, 0f, 1f);

            if (openAmmount != bookHelper.OpenAmmount)
            {
                float collisionZCenter, collisionZSize, collisionXSize, slowMultiplier, fastMultiplier;
                fastMultiplier = CalculateBezier(openAmmount, true);
                slowMultiplier = CalculateBezier(openAmmount, false);

                collisionXSize = 0.72f * fastMultiplier;
                collisionZSize = (0.35f * slowMultiplier) * -1;
                collisionZCenter = (0.25f * slowMultiplier) * -1;

                collider.size = new Vector3(colliderBaseX + collisionXSize, collider.size.y, colliderBaseZ + collisionZSize);
                collider.center = new Vector3(collider.center.x, collider.center.y, colliderBaseZC + collisionZCenter);

            }
            bookHelper.OpenAmmount = openAmmount;
        }

        public void EndTransform()
        {
            rb.isKinematic = false;
            if (bookHelper.OpenAmmount < 0.25)
            {
                close.Play();
                bookHelper.Close(0.5f);
                collider.size = new Vector3(colliderBaseX, collider.size.y, colliderBaseZ);
                collider.center = new Vector3(collider.center.x, collider.center.y, colliderBaseZC);


            }
            else if (bookHelper.OpenAmmount > 0.7)
            {
                open.Play();
                bookHelper.Open(0.5f);
                collider.size = new Vector3(colliderBaseX + 0.72f, collider.size.y, colliderBaseZ - 0.35f);
                collider.center = new Vector3(collider.center.x, collider.center.y, colliderBaseZC - 0.25f);
            }

            if (bookHelper.OpenAmmount > 0 && rb.useGravity)
            {
                rb.useGravity = false;
            }
            else if (bookHelper.OpenAmmount == 0 && !rb.useGravity)
            {
                //rb.useGravity = true;
                //rb.isKinematic = false;
            }

        }

        // Calculate the Bezier value for a given 't' using specified control points
        public static float CalculateBezier(float t, bool fast)
        {
            // Fixed start and end points
            Point p0 = new Point(0, 0);
            Point p1 = new Point(0.3f, 0.1f);
            Point p2 = new Point(0.7f, 0.3f);
            Point p3 = new Point(1, 1);

            if (fast)
            {
                p1 = new Point(0.3f, 0.7f);
                p2 = new Point(0.7f, 0.9f);
            }



            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            // Calculate the Bezier curve point at 't'
            Point p = uuu * p0; //first term
            p += 3 * uu * t * p1; //second term
            p += 3 * u * tt * p2; //third term
            p += ttt * p3; //fourth term

            return p.Y;
        }
    }
}