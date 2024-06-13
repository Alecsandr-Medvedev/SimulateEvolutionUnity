using System.Collections;
using System.Collections.Generic;

public class Field
{
    private float[] baseSettings_;
    private List<Organism> organismes_;
    List<GameObjectM> zonesO_;
    List<Zone> zonesZ_;

    public Field(float[] baseSettings)
    {
        baseSettings_ = baseSettings;
        organismes_ = new List<Organism> { };
        zonesO_ = new List<GameObjectM> { };
        zonesZ_ = new List<Zone> { };
    }

    public void addOrganism(Organism org)
    {
        organismes_.Add(org);
    }

    public void addZone(Zone zone)
    {
        zonesO_.Add(zone);
        zonesZ_.Add(zone);
    }

    public List<GameObjectM> getZones()
    {
        return zonesO_;
    }

    public List<Organism> FindOrganisms(Rect findZone)
    {
        List<Organism> orgs = new List<Organism> { };

        foreach (Organism org in organismes_)
        {
            if (org.getRect().Intersects(findZone))
            {
                orgs.Add(org);
            } 
        }

        return orgs;

    }

    public void delZone(ulong id)
    {
        int i = 0;
        foreach (Zone zone in zonesZ_)
        {
            
            if (zone.getId() == id)
            {
                zonesZ_.Remove(zone);
                break;
            }
            i++;
        }
        zonesO_.RemoveAt(i);
    }

    public float[] getSettings(float x, float y)
    {
        Rect r = new Rect(x, y, 1, 1);
        foreach (Zone zone in zonesZ_)
        {
            if (zone.getRect().Intersects(r))
            {
                return zone.getSettings();
            }
        }
        return baseSettings_;
    }

    public void delOrganism(ulong id)
    {
        foreach (Organism org in organismes_)
        {
            if (org.getId() == id)
            { 
                organismes_.Remove(org);
                break;
            }
        }
    }
}
