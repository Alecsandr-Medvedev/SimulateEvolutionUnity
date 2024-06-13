using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Organism : GameObjectM
{
    private float maxSpeed_, growthRate_, pointPh_, pointPr_, energy_, size_;
    private NeuralNetwork neuralGo_, neuralChoice_;
    
    float[] gens_;
    public InfoBorn infoBorn_;
    

    public Organism(float x, float y, float size, ulong id, float[] gens, float[][][] weight_neural_choice= null, float[][][]  weight_neural_go = null) : base (new Rect(x - (size / 2), y - (size / 2), size, size),
        Settings.getGensToColor(gens), id, 2)
    {
        pointPr_ = gens[0];
        pointPh_ = gens[1];
        maxSpeed_ = gens[2];
        growthRate_ = gens[3];
        neuralGo_ = new NeuralNetwork(Settings.SSN_go, Activations.Tanh, weight_neural_go);
        neuralChoice_ = new NeuralNetwork(Settings.SSN_choice, Activations.Sigmoid, weight_neural_choice);
        energy_ = size * Settings.kBornEnergy * 100;
        size_ = size;
        
        gens_ = gens;
        infoBorn_ = new InfoBorn(0, 0, 0, gens, weight_neural_choice, weight_neural_choice);
        inField();
        
    }
    public Organism(Rect rect, ulong id, float[] gens, float[][][] weight_neural_choice = null, float[][][] weight_neural_go = null) : base(rect,
        new float[4] { gens[0] / 100, gens[1] / 100, gens[2] / 2, (gens[3] / 2) + 0.5f }, id, 2)
    {
        pointPr_ = gens[0];
        pointPh_ = gens[1];
        maxSpeed_ = gens[2];
        growthRate_ = gens[3];
        neuralGo_ = new NeuralNetwork(Settings.SSN_go, Activations.Tanh, weight_neural_go);
        neuralChoice_ = new NeuralNetwork(Settings.SSN_choice, Activations.Sigmoid, weight_neural_choice);
        energy_ = rect.Width() * Settings.kBornEnergy * 100;
        size_ = rect.Width();

        gens_ = gens;
        infoBorn_ = new InfoBorn(0, 0, 0, gens, weight_neural_choice, weight_neural_choice);
        inField();

    }

    private void inField()
    {
        float x = getRect().X();
        float y = getRect().Y();
        if (x < 0)
        {
            MoveTo(Settings.WIDTH + x, y);
        }
        else if (x > Settings.WIDTH)
        {
            MoveTo(x - Settings.WIDTH, y);
        }

        x = getRect().X();
        if (y < 0)
        {
            MoveTo(x, Settings.HEIGHT + y);
        }
        else if (y > Settings.HEIGHT)
        {
            MoveTo(x, y - Settings.HEIGHT);
        }
    }

    public float[] getGens()
    {
        return gens_;
    }

    private void growth()
    {
        energy_ = energy_ - ((size_ * size_) * Settings.kEnergyGrow);
        size_ += (float) (growthRate_ * Settings.kGrow * Math.Sqrt(size_));
        setSize(size_, size_);

        if (energy_ <= 0)
        {
            is_alive_ = false;
        }
    }

    private bool compareGens(float[] gens)
    {
        float s = 0;
        s += Math.Abs(gens[0] - gens_[0]);
        s += Math.Abs(gens[1] - gens_[1]);
        s += Math.Abs(gens[2] - gens_[2]) * 50;
        s += Math.Abs(gens[3] - gens_[3]) * 100;

        if (s < Settings.kEnemy) return true;
        return false;
    }

    private List<Organism>[] detectOrganisms()
    {
        float findSize = size_ * Settings.kFind;
        Rect r = new Rect(rect_.XC() - (findSize / 2), rect_.YC() - (findSize / 2), findSize, findSize);
        List<Organism> fined_organisms = Settings.field.FindOrganisms(r);
        List<Organism> near_organisms = Settings.field.FindOrganisms(rect_);

        List<Organism> enemy_near_organisms = new List<Organism> { };
        List<Organism> friend_near_organisms = new List<Organism> { };
        List<Organism> enemy_fined_organisms = new List<Organism> { };
        List<Organism> friend_fined_organisms = new List<Organism> { };

        foreach (Organism el in fined_organisms) {
            if (el.getId() == id_) continue;

            if (compareGens(el.getGens())) friend_fined_organisms.Add(el);

            else enemy_fined_organisms.Add(el);
        }
        foreach (Organism el in near_organisms)
        {
            if (el.getId() == id_) continue;
            if (compareGens(el.getGens())) friend_near_organisms.Add(el);
            else enemy_near_organisms.Add(el);
        }



        return new List<Organism>[] { enemy_near_organisms, friend_near_organisms, enemy_fined_organisms, friend_fined_organisms };
    }

    private void Multiply(Organism parent)
    {
        energy_ *= (Settings.kBornEnergy / 100);
        float[] gens_p = parent.getGens();

        Random random = new Random();

        float newPointPr = Math.Max(Math.Min(100, (gens_[0] + gens_p[0]) / 2 + (float)(random.NextDouble() * Settings.kDeltMutate - 2 * Settings.kDeltMutate)), 0);
        float newPointPh = 100 - newPointPr;
        float newMaxSpeed = Math.Max(0.01f, Math.Min(2, (gens_[2] + gens_p[2]) / 2 + (float)(random.NextDouble() * Settings.kDeltMutate - 2 * Settings.kDeltMutate) / 50));
        float newGrowthRate = Math.Max(0.01f, Math.Min(1, (gens_[3] + gens_p[3]) / 2 + (float)(random.NextDouble() * Settings.kDeltMutate - 2 * Settings.kDeltMutate) / 100));

        float[][][] newNeuralChoice = neuralChoice_.GetMutateWeights();
        float[][][] newNeuralGo = neuralGo_.GetMutateWeights();
        float x = (parent.getRect().X() + rect_.X()) / 2;
         float y = (parent.getRect().Y() + rect_.Y()) / 2;
         float size = size_ * Settings.kBornEnergy / 100;
         if (size < Settings.minSizeOrganism)
         {
             size = Settings.minSizeOrganism;
         }

        infoBorn_.resetElements(x, y, size, new float[4] { newPointPr, newPointPh, newMaxSpeed, newGrowthRate }, newNeuralChoice, newNeuralGo);
        infoBorn_.setBorn(true);

    }

    public InfoBorn GetBorn()
    {
        return infoBorn_;
    }

    public float takeHit(float force)
    {
        if (size_ < force)
        {
            if (size_ > 0)
            {
                is_alive_ = false;
                return size_;
            }
            return 0;
        }
        else
        {
            size_ = size_ - force;
            return force;
        }
    }

    private void Attack(Organism org)
    {
        energy_ += org.takeHit(size_ * pointPr_) * Settings.kPredator;
    }

    private void Photosintesys(float illumination, int countNeighbors)
    {
        energy_ += (size_ * pointPh_ * illumination * Settings.kPhotosintes) / (countNeighbors + 1);
    }

    private void Walk(List<Organism> enemy_orgs, List<Organism> friend_orgs, float t, float v, float i)
    {
        float x_F = 0, y_F = 0, size_F = 0, x_E = 0, y_E = 0, size_E = 0, sr_XF = 0, sr_YF = 0, sr_XE = 0, sr_YE = 0;
        int count_F = friend_orgs.Count;
        int count_E = enemy_orgs.Count;
        foreach (Organism org in friend_orgs)
        {
            Rect r = org.getRect();
            x_F += r.X();
            y_F += r.Y();
            size_F = r.Width();
        }


        foreach (Organism org in enemy_orgs)
        {
            Rect r = org.getRect();
            x_E += r.X();
            y_E += r.Y();
            size_E = r.Width();
        }

        if (count_F == 0) count_F = 1;

        if (count_E == 0) count_E = 1;
        sr_XF = 1 - (rect_.X() - x_F / count_F) / Settings.WIDTH;
        sr_YF = 1 - (rect_.Y() - y_F / count_F) / Settings.HEIGHT;
        sr_XE = 1 - (rect_.X() - x_E / count_E) / Settings.WIDTH;
        sr_YE = 1 - (rect_.Y() - y_E / count_E) / Settings.HEIGHT;

        float[] input_ = new float[9] {sr_XF, sr_YF, size_F / count_F, sr_XE, sr_YE,
                  size_E / count_E, t, v, i };
        float[] output = neuralGo_.FeedForward(input_);
        float sqrt_size = (float) Math.Sqrt(size_);
        float move_x = output[0] * maxSpeed_ * sqrt_size;
        float move_y = output[1] * maxSpeed_ * sqrt_size;
        Move(move_x, move_y);
        inField();

        energy_ = energy_ - ((move_x + move_y) * Settings.kGo * sqrt_size);
    }

    private void Think()
    {
        float[] settings_point = Settings.field.getSettings(rect_.XC(), rect_.YC());

        List<Organism>[] finedOrganisms = detectOrganisms();

        float[] input_data = new float[8] {finedOrganisms[0].Count, finedOrganisms[1].Count, finedOrganisms[2].Count, finedOrganisms[3].Count,
                      settings_point[0], settings_point[1], settings_point[2], energy_ / size_ };
        float[] res = neuralChoice_.FeedForward(input_data);
        float mult = res[0];
        float atta = res[1];
        float phot = res[2];
        float walk = res[3];
        int countSee = finedOrganisms[2].Count + finedOrganisms[3].Count;
        int countNear = finedOrganisms[0].Count + finedOrganisms[1].Count;
        if (mult > atta && mult > phot && mult > walk && finedOrganisms[1].Count != 0 && countNear + countSee < Settings.kMaxNeighbour)
        {

            Multiply(finedOrganisms[1][0]);
/*            UnityEngine.Debug.Log(1);*/

        }
        else if (atta > phot && atta > walk && finedOrganisms[0].Count != 0)
        {
            Attack(finedOrganisms[0][0]);
       }
        else if (phot > walk)
        {
            Photosintesys(settings_point[2], countNear);
        }
        else
        {
            Walk(finedOrganisms[0].Concat(finedOrganisms[2]).ToList<Organism>(), finedOrganisms[1].Concat(finedOrganisms[3]).ToList<Organism>(), settings_point[0], settings_point[1], settings_point[2]);        }
            
    }

    public void Update()
    {
        infoBorn_.setBorn(false);
        growth();
        Think();
    }

    public float getEnergy()
    {
        return energy_;
    }
    public float getSize()
    {
        return size_;
    }

    public void setSelect(bool flag)
    {
        
        isSelect = flag;
    }

}

public struct InfoBorn
{
    private float x_;
    private float y_;
    private float size_;
    private float[] gens_;
    private float[][][] neuralChoice_;
    private float[][][] neuralGo_;
    bool isBorn_;

    public InfoBorn(float x, float y, float size, float[] gens, float[][][] neuralChoice, float[][][] neuralGo)
    {
        x_ = x;
        y_ = y;
        size_ = size;
        gens_ = gens;
        neuralChoice_ = neuralChoice;
        neuralGo_ = neuralGo;
        isBorn_ = false;
    }

    public void resetElements(float x, float y, float size, float[] gens, float[][][] neuralChoice, float[][][] neuralGo)
    {
        x_ = x;
        y_ = y;
        size_ = size;
        gens_ = gens;
        neuralChoice_ = neuralChoice;
        neuralGo_ = neuralGo;
        isBorn_ = false;
    }

    public Rect getRect()
    {
        return new Rect(x_, y_, size_, size_);
    }

    public float[] getGens()
    {
        return gens_;
    }

    public float getPointPr()
    {
        return gens_[0];
    }
    public float getPointPh()
    {
        return gens_[1];
    }
    public float getMaxSpeed()
    {
        return gens_[2];
    }
    public float getGrowthRate()
    {
        return gens_[3];
    }

    public float[][][] getNeuralChoice()
    {
        return neuralChoice_;
    }

    public float[][][] getNeuralGo()
    {
        return neuralGo_;
    }

    public void setBorn(bool isBorn)
    {
        isBorn_ = isBorn;
    }

    public bool getBorn()
    {
        return isBorn_;
    }
}
[System.Serializable]
public struct SaveOrganism
{
    private string name_;
    private float size_;
    private float[] gens_;
    private float[][][] neuralChoice_;
    private float[][][] neuralGo_;

    public SaveOrganism(string name, float size, float[] gens, float[][][] neuralChoice, float[][][] neuralGo)
    {
        name_ = name;
        size_ = size;
        gens_ = gens;
        neuralChoice_ = neuralChoice;
        neuralGo_ = neuralGo;
    }

    public string getName()
    {
        return name_;
    }
    public float getSize()
    {
        return size_;
    }

    public float[] getGens()
    {
        return gens_;
    }
    public float[][][] getNeuralChoice()
    {
        return neuralChoice_;
    }
    public float[][][] genNeuralGo()
    {
        return neuralGo_;
    }
}