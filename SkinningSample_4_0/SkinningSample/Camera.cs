#region File Description
//-----------------------------------------------------------------------------
// ChaseCamera.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
#endregion

namespace SmellOfRevenge2011
{
    public class Camera
    {
        public float dAspect = 0.0f;
        public float dFov = 0.0f;
        public Matrix transform;
        public float fovDiv = 1.0f;
        public float dX = 0.0f;
        //public float dY = 300.0f;
        //public float dZ = -1400.0f;
        public float dY = 0.0f;
        public float dZ = -10.0f;
        public float lX = 0.0f;
        //public float lY = 100.0f;
        //public float lZ = -250.0f;
        public float lY = 0.0f;
        public float lZ = 100.0f;
        private float fakeNearPlaneDistance = 1.0f;
        public float FakeNearPlaneDistance
        {
            get
            {
                return fakeNearPlaneDistance;
            }
            set
            {
                fakeNearPlaneDistance = value;
            }
        }
        private float fakeFarPlaneDistance = 1000.0f;
        public float FakeFarPlaneDistance
        {
            get
            {
                return fakeFarPlaneDistance;
            }
            set
            {
                fakeFarPlaneDistance = value;
            }
        }
        protected BoundingFrustum fakeCameraFrustum;
        public BoundingFrustum FakeCameraFrustum
        {
            get
            {
                return fakeCameraFrustum;
            }
            set
            {
                fakeCameraFrustum = value;
            }
        }

        protected BoundingFrustum cameraFrustum;
        public BoundingFrustum CameraFrustum
        {
            get
            {
                return cameraFrustum;
            }
            set
            {
                cameraFrustum = value;
            }
        }
        /// <summary>
        /// 0 is follow 1 is steady
        /// </summary>
        protected int state;
        public int State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }
        protected Vector3 newLookAt;
        public Vector3 NewLookAt
        {
            get
            {
                return newLookAt;
            }
            set
            {
                newLookAt = value;
            }
        }
        #region Chased object properties (set externally each frame)

        /// <summary>
        /// Position of object being chased.
        /// </summary>
        public Vector3 ChasePosition
        {
            get { return chasePosition; }
            set { chasePosition = value; }
        }
        private Vector3 chasePosition;

        /// <summary>
        /// Direction the chased object is facing.
        /// </summary>
        public Vector3 ChaseDirection
        {
            get { return chaseDirection; }
            set { chaseDirection = value; }
        }
        private Vector3 chaseDirection;

        /// <summary>
        /// Chased object's Up vector.
        /// </summary>
        public Vector3 Up
        {
            get { return up; }
            set { up = value; }
        }
        private Vector3 up = Vector3.Up;

        #endregion

        #region Desired camera positioning (set when creating camera or changing view)

        /// <summary>
        /// Desired camera position in the chased object's coordinate system.
        /// </summary>
        public Vector3 DesiredPositionOffset
        {
            get { return desiredPositionOffset; }
            set { desiredPositionOffset = value; }
        }
        private Vector3 desiredPositionOffset = new Vector3(0, 2.0f, 2.0f);

        /// <summary>
        /// Desired camera position in world space.
        /// </summary>
        public Vector3 DesiredPosition
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return desiredPosition;
            }
            set
            {
                desiredPosition = value;
            }
        }
        private Vector3 desiredPosition;

        /// <summary>
        /// Look at point in the chased object's coordinate system.
        /// </summary>
        public Vector3 LookAtOffset
        {
            get { return lookAtOffset; }
            set { lookAtOffset = value; }
        }
        private Vector3 lookAtOffset = new Vector3(0, 2.8f, 0);

        /// <summary>
        /// Look at point in world space.
        /// </summary>
        public Vector3 LookAt
        {
            get
            {
                // Ensure correct value even if update has not been called this frame
                UpdateWorldPositions();

                return lookAt;

            }

        }
        private Vector3 lookAt;

        #endregion

        #region Camera physics (typically set when creating camera)

        /// <summary>
        /// Physics coefficient which controls the influence of the camera's position
        /// over the spring force. The stiffer the spring, the closer it will stay to
        /// the chased object.
        /// </summary>
        public float Stiffness
        {
            get { return stiffness; }
            set { stiffness = value; }
        }
        private float stiffness = 1800.0f;

        /// <summary>
        /// Physics coefficient which approximates internal friction of the spring.
        /// Sufficient damping will prevent the spring from oscillating infinitely.
        /// </summary>
        public float Damping
        {
            get { return damping; }
            set { damping = value; }
        }
        private float damping = 600.0f;

        /// <summary>
        /// Mass of the camera body. Heaver objects require stiffer springs with less
        /// damping to move at the same rate as lighter objects.
        /// </summary>
        public float Mass
        {
            get { return mass; }
            set { mass = value; }
        }
        private float mass = 50.0f;

        #endregion

        #region Current camera properties (updated by camera physics)

        /// <summary>
        /// Position of camera in world space.
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
        }
        private Vector3 position;

        /// <summary>
        /// Velocity of camera.
        /// </summary>
        public Vector3 Velocity
        {
            get { return velocity; }
        }
        private Vector3 velocity;

        #endregion


        #region Perspective properties

        /// <summary>
        /// Perspective aspect ratio. Default value should be overriden by application.
        /// </summary>
        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }
        private float aspectRatio = 4.0f / 3.0f;

        /// <summary>
        /// Perspective field of view.
        /// </summary>
        public float FieldOfView
        {
            get { return fieldOfView; }
            set { fieldOfView = value; }
        }
        // private float fieldOfView = MathHelper.ToRadians(45.0f);
        private float fieldOfView = (MathHelper.PiOver4);
        /// <summary>
        /// Distance to the near clipping plane.
        /// </summary>
        public float NearPlaneDistance
        {
            get { return nearPlaneDistance; }
            set { nearPlaneDistance = value; }
        }
        private float nearPlaneDistance = 1.0f;

        /// <summary>
        /// Distance to the far clipping plane.
        /// </summary>
        public float FarPlaneDistance
        {
            get { return farPlaneDistance; }
            set { farPlaneDistance = value; }
        }
        private float farPlaneDistance = 10000.0f;

        #endregion

        #region orthographic




        #endregion

        #region Matrix properties

        /// <summary>
        /// View transform matrix.
        /// </summary>
        public Matrix View
        {
            get { return view; }
        }
        private Matrix view;

        /// <summary>
        /// Projecton transform matrix.
        /// </summary>
        public Matrix Projection
        {
            get { return projection; }
        }
        private Matrix projection;

        private Matrix orthographic;
        public Matrix Orthographic
        {
            get
            {
                return orthographic;
            }
        }
        #endregion


        #region Methods

        /// <summary>
        /// Rebuilds object space values in world space. Invoke before publicly
        /// returning or privately accessing world space values.
        /// </summary>
        private void UpdateWorldPositions()
        {
            //Normally Vector3.Up
            // Construct a matrix to transform from object space to worldspace
            Up = new Vector3(0.0f, 1.0f, -0.0f);
             transform = Matrix.Identity;
            transform.Forward = ChaseDirection;
            transform.Up = Up;
            transform.Right = Vector3.Cross(Up, ChaseDirection);
            
            // Calculate desired camera properties in world space
            if (state == 0) //follow ball
            {
                desiredPosition = ChasePosition +
                    Vector3.TransformNormal(DesiredPositionOffset, transform);
                lookAt = ChasePosition +
                    Vector3.TransformNormal(LookAtOffset, transform);

            }
            if (state == 1)
            {
                desiredPosition = ChasePosition;
                //dZ = 0;
                //lZ = 0;
                //dY = 200;
                //lY = 0;

                //desiredPositionOffset = new Vector3(dX, dY, dZ);
               // lookAtOffset = new Vector3(lX, lY, lZ);

                //Console.WriteLine(desiredPositionOffset);
               // Console.WriteLine(lookAtOffset);
                desiredPosition = ChasePosition + Vector3.TransformNormal(DesiredPositionOffset, transform);
                lookAt = newLookAt + Vector3.TransformNormal(LookAtOffset, transform);


            }
            //cameraFrustum.Matrix = view * projection;
            transform.Translation = desiredPosition;
        }

        public Matrix FakeProjection
        {
            get
            {
                return fakeProjection;
            }
            set
            {
                fakeProjection = value;
            }
        }
        private Matrix fakeProjection;

        /// <summary>
        /// Rebuilds camera's view and projection matricies.
        /// </summary>
        private void UpdateMatrices()
        {
            view = Matrix.CreateLookAt(this.Position, this.LookAt, this.Up);
            //projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView,
            //    AspectRatio, NearPlaneDistance, FarPlaneDistance);

            aspectRatio = ScreenManager.ga.CurrentDisplayMode.Width/ScreenManager.ga.CurrentDisplayMode.Height;
            //changed AUgust
            //aspectRatio = 1.0f;
            fieldOfView = MathHelper.PiOver4;
           // fieldOfView = 1;
            projection = Matrix.CreatePerspectiveFieldOfView(FieldOfView + dFov, AspectRatio + dAspect, NearPlaneDistance, FarPlaneDistance);

            //Difference 800 480
           // projection = Matrix.CreateOrthographic(800.0f * 1.0f, 480.0f * 1.0f, -farPlaneDistance, farPlaneDistance);
           // projection = Matrix.CreatePerspective(200, 200, 1, 10000);
          //  orthographic = Matrix.CreateOrthographic(800, 480, 1.0f, 1000.0f);
           // projection = orthographic;

           // projection = Matrix.CreateOrthographic(2000, 400, 1, 2000);
            //view = Matrix.CreateLookAt(
    //new Vector3(0.0f, 0.0f, 1.0f),
    //new Vector3(ScreenManager.ps1.World.Translation.X, 0.0f, 0.0f),
    //Vector3.Zero,
    //Vector3.Up);
    
            //projection = Matrix.CreateOrthographic(800, 480.0f, nearPlaneDistance, farPlaneDistance);
            fakeProjection = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, fakeNearPlaneDistance, fakeFarPlaneDistance);

            cameraFrustum.Matrix = view * projection;
            fakeCameraFrustum.Matrix = view * fakeProjection;
        }

        /// <summary>
        /// Forces camera to be at desired position and to stop moving. The is useful
        /// when the chased object is first created or after it has been teleported.
        /// Failing to call this after a large change to the chased object's position
        /// will result in the camera quickly flying across the world.
        /// </summary>
        public void Reset()
        {
            UpdateWorldPositions();

            cameraFrustum = new BoundingFrustum(Matrix.Identity);
            fakeCameraFrustum = new BoundingFrustum(Matrix.Identity);
            // Stop motion
            velocity = Vector3.Zero;

            // Force desired position
            position = desiredPosition;

            UpdateMatrices();
        }

        /// <summary>
        /// Animates the camera from its current position towards the desired offset
        /// behind the chased object. The camera's animation is controlled by a simple
        /// physical spring attached to the camera and anchored to the desired position.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            if (gameTime == null)
                throw new ArgumentNullException("gameTime");

            UpdateWorldPositions();


            fakeCameraFrustum = new BoundingFrustum(Matrix.Identity);
            cameraFrustum = new BoundingFrustum(Matrix.Identity);
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate spring force
            Vector3 stretch = position - desiredPosition;
            Vector3 force = -stiffness * stretch - damping * velocity;

            // Apply acceleration
            Vector3 acceleration = force / mass;
            velocity += acceleration * elapsed;

            // Apply velocity
            position += velocity * elapsed;

            UpdateMatrices();
        }

        #endregion
    }
}
