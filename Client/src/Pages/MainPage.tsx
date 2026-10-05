import { useEffect, useState } from "react";
import logo from "../RamRanchPictureReal.jpg";
import {Api, type ListingResponse } from "../api/Api";

const MyApi = new Api();
export default function MainPage() {
    const [listings, setListings] = useState<ListingResponse[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        MyApi.getListings.listingGetListings()
            .then(r => setListings(r.data))
            .catch(e => setError(e?.error?.title ?? "Could not load listings"))
            .finally(() => setLoading(false));
    }, []);

    if (loading) return <p>Loading listings...</p>;
    if (error) return <p>{error}</p>;
    if (listings.length === 0) return <p>No listings yet.</p>;

    return (
        <>
            <header className="site-header">
                <img src={logo} alt="Ram Ranch welcome mat" className="site-logo" />
                <h1>Ram Ranch Road</h1>
            </header>

            <main className="container">
                <h2 className="section-title">All listings</h2>
                <ul className="listings">
                    {listings.map(l => (
                        <li key={l.listingId} className="card">
                            <span className="badge">{l.categoryName}</span>
                            <h3>{l.title}</h3>
                            <p className="description">{l.description}</p>
                            <div className="card-footer">
                                <span className="price">{l.price} kr</span>
                                <span className="stock">{l.stock} in stock</span>
                            </div>
                            <p className="vendor">Sold by {l.vendorName}</p>
                        </li>
                    ))}
                </ul>
            </main>
        </>
    );
}