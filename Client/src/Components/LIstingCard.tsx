import { useState } from "react";
import type { ListingResponse } from "../api/Api";

type Props = {
    listing: ListingResponse;
    isOwner: boolean;
    onEdit: () => void;
    onDelete: () => void;
    onBuy: (quantity: number) => Promise<void>;
};

export default function ListingCard({ listing: l, isOwner, onEdit, onDelete, onBuy }: Props) {
    const [quantity, setQuantity] = useState(1);
    const [buying, setBuying] = useState(false);

    const stock = l.stock ?? 0;
    const soldOut = stock === 0;
    const total = (l.price ?? 0) * quantity;

    const handleBuy = async () => {
        setBuying(true);
        try {
            await onBuy(quantity);
            setQuantity(1);
        } finally {
            setBuying(false);
        }
    };

    return (
        <li className="card">
            <span className="badge">{l.categoryName}</span>
            <h3>{l.title}</h3>
            <p className="description">{l.description}</p>
            <div className="card-footer">
                <span className="price">{l.price} kr</span>
                <span className="stock">{soldOut ? "Sold out" : `${stock} in stock`}</span>
            </div>
            <p className="vendor">Sold by {l.vendorName}</p>

            {isOwner ? (
                <div className="card-actions">
                    <button type="button" onClick={onEdit}>Edit</button>
                    <button type="button" className="danger" onClick={onDelete}>Delete</button>
                </div>
            ) : (
                <div className="card-actions">
                    <input
                        type="number"
                        className="qty-input"
                        min={1}
                        max={stock}
                        step={1}
                        value={quantity}
                        disabled={soldOut || buying}
                        onChange={e => setQuantity(Math.max(1, Math.min(stock, Number(e.target.value) || 1)))}
                    />
                    <button type="button" disabled={soldOut || buying} onClick={handleBuy}>
                        {buying ? "Ordering..." : soldOut ? "Sold out" : `Buy · ${total} kr`}
                    </button>
                </div>
            )}
        </li>
    );
}