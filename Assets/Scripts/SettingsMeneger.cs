using System.Collections;
using System.Collections.Generic;

public class SettingsMeneger : UnityEngine.MonoBehaviour
{
    public FieldInterface FPS_;
    public FieldInterface TPS_;
    public FieldInterface camSpeed_;
    public FieldInterface minZoom_;
    public FieldInterface maxZoom_;
    public FieldInterface speedZoom_;
    public FieldInterface kBornEnergy_;
    public FieldInterface kEnergyGrow_;
    public FieldInterface kGrow_;
    public FieldInterface kFind_;
    public FieldInterface kEnemy_;
    public FieldInterface kMaxNeighbour_;
/*    public FieldInterface kMultiply_;*/
    public FieldInterface kDeltMutate_;
    public FieldInterface kPhotosintes_;
    public FieldInterface kPredator_;
    public FieldInterface kGo_;
    public FieldInterface minSizeOrganism_;
    public FieldInterface maxSizeOrgainsm_;
    public FieldInterface COG_;
    public FieldInterface CKG_;
    public FieldInterface CZG_;
    public FieldInterface sizeZones_;
    public FieldInterface spreadZones_;

    private void Awake()
    {
        FPS_.Initialized("FPS", Settings.FPS.ToString());
        TPS_.Initialized("TPS", Settings.TPS.ToString());
        camSpeed_.Initialized("camSpeed", Settings.camSpeed.ToString());
        minZoom_.Initialized("minZoom", Settings.minZoom.ToString());
        maxZoom_.Initialized("maxZoom", Settings.maxZoom.ToString());
        speedZoom_.Initialized("speedZoom", Settings.speedZoom.ToString());
        kBornEnergy_.Initialized("kBornEnergy", Settings.kBornEnergy.ToString());
        kEnergyGrow_.Initialized("kEnergyGrow", Settings.kEnergyGrow.ToString());
        kGrow_.Initialized("kGrow", Settings.kGrow.ToString());
        kFind_.Initialized("kFind", Settings.kFind.ToString());
        kEnemy_.Initialized("kEnemy", Settings.kEnemy.ToString());
        kMaxNeighbour_.Initialized("kMaxNeighbour", Settings.kMaxNeighbour.ToString());
/*        kMultiply_.Initialized("kMultiply", Settings.kMultiply.ToString());
*/        kDeltMutate_.Initialized("kDeltMutate", Settings.kDeltMutate.ToString());
        kPhotosintes_.Initialized("kPhotosintes", Settings.kPhotosintes.ToString());
        kPredator_.Initialized("kPredator", Settings.kPredator.ToString());
        kGo_.Initialized("kGo", Settings.kGo.ToString());
        minSizeOrganism_.Initialized("minSizeOrganism", Settings.minSizeOrganism.ToString());
        maxSizeOrgainsm_.Initialized("maxSizeOrgainsm", Settings.maxSizeOrgainsm.ToString());
        COG_.Initialized("COG", Settings.COG.ToString());
        CKG_.Initialized("CKG", Settings.CKG.ToString());
        CZG_.Initialized("CZG", Settings.CZG.ToString());
        sizeZones_.Initialized("sizeZones", Settings.sizeZones.ToString());
        spreadZones_.Initialized("spreadZones", Settings.spreadZones.ToString());
    }

}
