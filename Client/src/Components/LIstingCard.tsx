import type { ListingResponse } from "../api/Api";

type Props = {
    listing: ListingResponse;
    isOwner: boolean;
    onEdit: () => void;
    onDelete: () => void;
};

export default function ListingCard({ listing: l, isOwner, onEdit, onDelete }: Props) {
    return (
        <li className="card">
            <span className="badge">{l.categoryName}</span>
            <h3>{l.title}</h3>
            <p className="description">{l.description}</p>
            <div className="card-footer">
                <span className="price">{l.price} kr</span>
                <span className="stock">{l.stock} in stock</span>
            </div>
            <p className="vendor">Sold by {l.vendorName}</p>

            {isOwner && (
                <div className="card-actions">
                    <button type="button" onClick={onEdit}>Edit</button>
                    <button type="button" className="danger" onClick={onDelete}>Delete</button>
                </div>
            )}
        </li>
    );
}