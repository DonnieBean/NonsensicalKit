namespace NonsensicalKit.PhysicsLogic
{
    public class CollisionGrabber : Grabber
    {
        protected override void OnEnter(Materials materials)
        {
            base.OnEnter(materials);
            if ( adsorbMaterials != null)
            {
                return;
            }
            if (materials is CollisionMaterials)
            {
                if (materials.CanAdsorb)
                {
                    adsorbMaterials = materials;
                    SetPositionAndRotation();
                    adsorbMaterials.CrtGrabber = this;

                    Dormancy();
                    adsorbMaterials.Activation();
                }
            }
        }

        public virtual void MaterialsSet()
        {
            adsorbMaterials = null;
            Activation();
        }
    }
}
