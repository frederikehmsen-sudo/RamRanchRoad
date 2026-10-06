import { useEffect, useState, type FormEvent } from "react";
import { Api, type CategoryResponse, type ListingResponse } from "../api/Api";

const MyApi = new Api();

type Props = {
    listing?: ListingResponse; 
    onSaved: (listing: ListingResponse) => void;
    onCancel?: () => void;
};

export default function ListingForm({ listing, onSaved, onCancel }: Props) {
    const isEdit = listing !== undefined;

    const [categories, setCategories] = useState<CategoryResponse[]>([]);
    const [title, setTitle] = useState(listing?.title ?? "");
    const [description, setDescription] = useState(listing?.description ?? "");
    const [price, setPrice] = useState(listing?.price != null ? String(listing.price) : "");
    const [stock, setStock] = useState(listing?.stock != null ? String(listing.stock) : "1");
    const [categoryId, setCategoryId] = useState(listing?.categoryId != null ? String(listing.categoryId) : "");
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        MyApi.getCategories.categoryGetCategories()
            .then(r => {
                setCategories(r.data);
                // In create mode, default to the first category
                if (!isEdit && r.data[0]?.id != null) setCategoryId(String(r.data[0].id));
            })
            .catch(() => setError("Could not load categories"));
    }, [isEdit]);

    const submit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setError(null);
        setSubmitting(true);

        const payload = {
            title,
            description,
            price: Number(price),
            stock: Number(stock),
            categoryId: Number(categoryId),
        };

        try {
            const res = isEdit
                ? await MyApi.updateListing.listingUpdateListing({
                    ...payload,
                    listingIdForLookup: listing.listingId,
                })
                : await MyApi.createListing.listingCreateListing(payload);

            onSaved(res.data);

            
            if (!isEdit) {
                setTitle("");
                setDescription("");
                setPrice("");
                setStock("1");
            }
        } catch (err: any) {
           
            setError(err?.error?.title ?? "Could not save listing");
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <form onSubmit={submit} className="listing-form">
            <h2 className="section-title">{isEdit ? "Edit listing" : "Create a listing"}</h2>

            <input
                placeholder="Title"
                value={title}
                onChange={e => setTitle(e.target.value)}
                required
            />
            <input
                placeholder="Description"
                value={description}
                onChange={e => setDescription(e.target.value)}
            />
            <input
                type="number"
                placeholder="Price (kr)"
                min="0.01"
                step="0.01"
                value={price}
                onChange={e => setPrice(e.target.value)}
                required
            />
            <input
                type="number"
                placeholder="Stock"
                min="0"
                step="1"
                value={stock}
                onChange={e => setStock(e.target.value)}
                required
            />
            <select value={categoryId} onChange={e => setCategoryId(e.target.value)} required>
                {categories.map(c => (
                    <option key={c.id} value={c.id}>{c.name}</option>
                ))}
            </select>

            <div className="form-actions">
                <button type="submit" disabled={submitting || !categoryId}>
                    {submitting ? "Saving..." : isEdit ? "Save changes" : "Create listing"}
                </button>
                {isEdit && onCancel && (
                    <button type="button" className="secondary" onClick={onCancel}>
                        Cancel
                    </button>
                )}
            </div>

            {error && <p className="form-error">{error}</p>}
        </form>
    );
}