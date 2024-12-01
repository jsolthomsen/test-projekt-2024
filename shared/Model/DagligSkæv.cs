namespace shared.Model;

public class DagligSkæv : Ordination {
    public List<Dosis> doser { get; set; } = new List<Dosis>();

    public DagligSkæv(DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel) : base(laegemiddel, startDen, slutDen) {
	}

    public DagligSkæv(DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel, Dosis[] doser) : base(laegemiddel, startDen, slutDen) {
        this.doser = doser.ToList();
    }    

    public DagligSkæv() : base(null!, new DateTime(), new DateTime()) {
    }

	public void opretDosis(DateTime tid, double antal) {
        doser.Add(new Dosis(tid, antal));
    }

	public override double samletDosis() {
		return (base.antalDage() + 1) * doegnDosis();
	}

	public override double doegnDosis()
	{
		double result = 0;
		if (doser.Count() > 0)
		{
			foreach (Dosis dose in doser)
			{
				result += dose.antal;
			} ;
		}
		return result;
	}

	public override String getType() {
		return "DagligSkæv";
	}
}
