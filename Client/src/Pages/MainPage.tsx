import { useEffect, useState } from "react";
import logo from "../RamRanchPictureReal.jpg";
import { Api, type ListingResponse, type UserResponse } from "../api/Api";
import ListingForm from "../components/ListingForm";
import ListingCard from "../components/ListingCard";

const MyApi = new Api();

export default function MainPage() {
    const [listings, setListings] = useState<ListingResponse[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [user, setUser] = useState<UserResponse | null>(null);
    const [editingId, setEditingId] = useState<number | null>(null);

    useEffect(() => {
        MyApi.getListings.listingGetListings()
            .then(r => setListings(r.data))
            .catch(e => setError(e?.error?.title ?? "Could not load listings"))
            .finally(() => setLoading(false));
    }, []);

    useEffect(() => {
        MyApi.getMe.userGetMe()
            .then(r => setUser(r.data))
            .catch(() => setUser(null));
    }, []);

    const handleCreated = (created: ListingResponse) => {
        setListings(prev => [...prev, created]);
    };

    const handleUpdated = (updated: ListingResponse) => {
        setListings(prev => prev.map(l => (l.listingId === updated.listingId ? updated : l)));
        setEditingId(null);
    };

    const handleDelete = async (listingId: number | undefined) => {
        if (listingId === undefined) return;
        if (!confirm("Delete this listing?")) return;
        try {
            await MyApi.deleteListings.listingDeleteListings({ listingId });
            setListings(prev => prev.filter(l => l.listingId !== listingId));
        } catch (e: any) {
            setError(e?.error?.title ?? "Could not delete listing");
        }
    };

    return (
        <>
            <header className="site-header">
                <img src={logo} alt="Ram Ranch welcome mat" className="site-logo" />
                <h1>Ram Ranch Road</h1>
            </header>

            <main className="container">
                <ListingForm onSaved={handleCreated} />

                <h2 className="section-title">Viewing all listings as {user && ` ${user.userName}`}</h2>

                {loading && <p>Loading listings...</p>}
                {error && <p className="form-error">{error}</p>}
                {!loading && !error && listings.length === 0 && <p>No listings yet.</p>}

                <ul className="listings">
                    {listings.map(l =>
                        editingId === l.listingId ? (
                            <li key={l.listingId} className="card">
                                <ListingForm
                                    listing={l}
                                    onSaved={handleUpdated}
                                    onCancel={() => setEditingId(null)}
                                />
                            </li>
                        ) : (
                            <ListingCard
                                key={l.listingId}
                                listing={l}
                                isOwner={user !== null && l.vendorId === user.userId}
                                onEdit={() => setEditingId(l.listingId ?? null)}
                                onDelete={() => handleDelete(l.listingId)}
                            />
                        )
                    )}
                </ul>
            </main>
        </>
    );
}