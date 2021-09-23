namespace NonsensicalKit.PhysicsLogic
{
    public class CollisionMaterials : Materials
    {
        public override bool CanSetModel { get { return true; } }

        public override bool CanAdsorb { get { return crtGrabber == null; } }

        protected override void OnEnter(Placement item)
        {
            base.OnEnter(item);

            if (crtPlacement!=null)
            {
                return;
            }

            if (item is CollisionPlacement && item.CanSet(physicsLogicName))
            {
                item.CrtMaterials = this;
                crtPlacement = item;
                Dormancy();
                if (crtGrabber != null && crtGrabber is CollisionGrabber)
                {
                    var v = crtGrabber as CollisionGrabber;
                    v.MaterialsSet();
                }
                crtGrabber = null;
            }
        }

        protected override void OnGrab()
        {
            base.OnGrab();

            if (crtPlacement != null)
            {
                crtPlacement.CrtMaterials = null;
                crtPlacement = null;
            }
        }
    }
}
