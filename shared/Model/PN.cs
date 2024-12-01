namespace shared.Model;

public class PN : Ordination {
	public double antalEnheder { get; set; }
    public List<Dato> dates { get; set; } = new List<Dato>();

    public PN (DateTime startDen, DateTime slutDen, double antalEnheder, Laegemiddel laegemiddel) : base(laegemiddel, startDen, slutDen) {
		this.antalEnheder = antalEnheder;
	}

    public PN() : base(null!, new DateTime(), new DateTime()) {
    }

    /// <summary>
    /// Registrerer at der er givet en dosis på dagen givesDen
    /// Returnerer true hvis givesDen er inden for ordinationens gyldighedsperiode og datoen huskes
    /// Returner false ellers og datoen givesDen ignoreres
    /// </summary>
    public bool givDosis(Dato givesDen) {
	    if (givesDen.dato >= startDen && givesDen.dato <= slutDen)
	    {
		    dates.Add(givesDen);
		    return true;
	    }
	    else
	    {
		    return false;   
	    }
    }

    public override double doegnDosis()
    {
	    double result = 0;
	    if (dates.Count() > 0)
	    {
		    var min = dates.First().dato.Date;
		    var max = dates.First().dato.Date;

		    foreach (var dato in dates)
		    {
			    if (dato.dato.Date < min)
			    {
				    min = dato.dato.Date;
			    }

			    if (dato.dato.Date > max)
			    {
				    max = dato.dato.Date;
			    }
		    }

		    int difference = (max - min).Days + 1;
		    result = samletDosis() / difference;
	    }

	    return result;

    }


    public override double samletDosis() {
        return dates.Count() * antalEnheder;
    }

    public int getAntalGangeGivet() {
        return dates.Count();
    }

	public override String getType() {
		return "PN";
	}
}
