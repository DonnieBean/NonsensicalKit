namespace NonsensicalKit.PhysicsLogic
{
    public abstract class SignalGrabber : Grabber
    {
        public virtual void Adsorb()
        {
            if (touchMaterials.Count == 0 || adsorbMaterials != null)
            {
                return;
            }
            adsorbMaterials = touchMaterials[0];
            if (adsorbMaterials.CanAdsorb == true)
            {
                SignalMaterials signalMaterials = adsorbMaterials as SignalMaterials;
                signalMaterials.Adsorbing(transform);

                Dormancy();

                if (signalMaterials.GetComponent<Materials>() != null)
                {
                    signalMaterials.GetComponent<Materials>().Activation();
                }
                else
                {
                    signalMaterials.Activation();
                }
            }
        }

        public virtual void Release()
        {
            if (adsorbMaterials == null )
            {
                return;
            }

            if (adsorbMaterials.CanSetModel == true)
            {
                SignalMaterials signalMaterials = adsorbMaterials as SignalMaterials;
                signalMaterials.SetModel();
                adsorbMaterials.Dormancy();
                adsorbMaterials = null;
                Activation();
            }
        }

    }
}
