import { useEffect, useState } from "react";
import logo from "../RamRanchPictureReal.jpg";
import { Api, type ListingResponse, type UserResponse } from "../api/Api";
import ListingForm from "../components/ListingForm";
import ListingCard from "../components/ListingCard";
import { MyApi } from "../api/client";
export default function MainPage() {
    const [listings, setListings] = useState<ListingResponse[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [user, setUser] = useState<UserResponse | null>(null);
    const [editingId, setEditingId] = useState<number | null>(null);
    const [notice, setNotice] = useState<{ type: "success" | "error"; text: string} | null>(null)
    const [showCreate, setShowCreate] = useState(false);
    const [categories, setCategories] = useState<CategoryResponse[]>([]);
    const [categoryFilter, setCategoryFilter] = useState<number | null>(null);
    const [showFilter, setShowFilter] = useState(false);

    useEffect(() => {
        MyApi.getCategories.categoryGetCategories()
            .then(r => setCategories(r.data))
            .catch(() => {});
    }, []);

    useEffect(() => {
        let cancelled = false;
        setLoading(true);
        setError(null);
        MyApi.getListings.listingGetListings({ categoryId: categoryFilter ?? undefined })
            .then(r => { if (!cancelled) setListings(r.data); })
            .catch(e => { if (!cancelled) setError(e?.error?.title ?? "Could not load listings"); })
            .finally(() => { if (!cancelled) setLoading(false); });
        return () => { cancelled = true; };
    }, [categoryFilter]);

    const activeCategory = categories.find(c => c.id === categoryFilter);

    const selectCategory = (id: number | null) => {
        setCategoryFilter(id);
        setShowFilter(false);
    };

        const buy = async (listing: ListingResponse, quantity: number) => {
            setNotice(null);
            try {
                const r = await MyApi.placeOrder.orderPlaceOrder({
                    listingId: listing.listingId,
                    quantity,
                });
                setListings(prev =>
                    prev.map(l =>
                        l.listingId === listing.listingId
                            ? {...l, stock: (l.stock ?? 0) - quantity}
                            : l
                    )
                );
                setNotice({
                    type: "success",
                    text: `Ordered ${r.data.quantity} × ${r.data.productTitle} for ${r.data.pricePaid} kr`,
                });
            } catch (e: any) {
                setNotice({type: "error", text: e?.error?.title ?? "Could not place order"});
            }
        };

        useEffect(() => {
            MyApi.getMe.userGetMe()
                .then(r => setUser(r.data))
                .catch(() => setUser(null));
        }, []);

        const handleCreated = (created: ListingResponse) => {
            if (categoryFilter === null || created.categoryId === categoryFilter) {
                setListings(prev => [...prev, created]);
            }
            setShowCreate(false);
            setNotice({type: "success", text: `Listed "${created.title}"`});
        };

        const handleUpdated = (updated: ListingResponse) => {
            setListings(prev => prev.map(l => (l.listingId === updated.listingId ? updated : l)));
            setEditingId(null);
        };

        const handleDelete = async (listingId: number | undefined) => {
            if (listingId === undefined) return;
            if (!confirm("Delete this listing?")) return;
            try {
                await MyApi.deleteListings.listingDeleteListings({listingId});
                setListings(prev => prev.filter(l => l.listingId !== listingId));
            } catch (e: any) {
                setError(e?.error?.title ?? "Could not delete listing");
            }
        };

        return (
            <>
                <header className="site-header">
                    <img src={logo} alt="Ram Ranch welcome mat" className="site-logo"/>
                    <h1>Ram Ranch Road</h1>

                    <div className="header-actions">
                        <div className="filter-wrapper">
                            <button
                                type="button"
                                className="secondary"
                                aria-haspopup="listbox"
                                aria-expanded={showFilter}
                                onClick={() => setShowFilter(s => !s)}
                            >
                                Filter: {activeCategory?.name ?? "All"} ▾
                            </button>

                            {showFilter && (
                                <>
                                    <div className="filter-backdrop" onClick={() => setShowFilter(false)}/>
                                    <ul className="filter-menu" role="listbox">
                                        <li>
                                            <button
                                                type="button"
                                                className={categoryFilter === null ? "active" : ""}
                                                onClick={() => selectCategory(null)}
                                            >
                                                All categories
                                            </button>
                                        </li>
                                        {categories.map(c => (
                                            <li key={c.id}>
                                                <button
                                                    type="button"
                                                    className={categoryFilter === c.id ? "active" : ""}
                                                    onClick={() => selectCategory(c.id ?? null)}
                                                >
                                                    {c.name}
                                                </button>
                                            </li>
                                        ))}
                                    </ul>
                                </>
                            )}
                        </div>

                        <button type="button" onClick={() => setShowCreate(true)}>
                            + Create listing
                        </button>
                    </div>
                </header>

                <main className="container">
                    <h2 className="section-title">
                        Viewing {activeCategory ? `${activeCategory.name} listings` : "all listings"} as {user?.userName}
                    </h2>

                    {notice && (
                        <p className={notice.type === "error" ? "form-error" : "form-success"}>{notice.text}</p>
                    )}

                    {loading && <p>Loading listings...</p>}
                    {error && <p className="form-error">{error}</p>}
                    {!loading && !error && listings.length === 0 && <p>No listings in this category.</p>}

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
                                    onBuy={quantity => buy(l, quantity)}
                                />
                            )
                        )}
                    </ul>
                </main>
                {showCreate && (
                    <div className="modal-backdrop" onClick={() => setShowCreate(false)}>
                        <div
                            className="modal"
                            role="dialog"
                            aria-modal="true"
                            onClick={e => e.stopPropagation()}
                        >
                            <ListingForm
                                onSaved={handleCreated}
                                onCancel={() => setShowCreate(false)}
                            />
                        </div>
                    </div>
                )}
            </>
        );
}