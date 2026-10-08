import { useState, type FormEvent } from "react";
import type { CategoryResponse } from "../api/Api";
import { MyApi } from "../api/client";

type Props = {
    onSaved: (category: CategoryResponse) => void;
    onCancel?: () => void;
};

export default function CategoryForm({ onSaved, onCancel }: Props) {
    const [name, setName] = useState("");
    const [submitting, setSubmitting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const submit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setError(null);
        setSubmitting(true);
        try {
            const res = await MyApi.createCategory.categoryCreateCategory({ name });
            onSaved(res.data);
            setName("");
        } catch (err: any) {
            setError(err?.error?.title ?? "Could not create category");
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <form onSubmit={submit} className="listing-form">
            <h2 className="section-title">Create a category</h2>
            <input
                placeholder="Category name"
                value={name}
                onChange={e => setName(e.target.value)}
                maxLength={50}
                required
            />
            <div className="form-actions">
                <button type="submit" disabled={submitting}>
                    {submitting ? "Saving..." : "Create category"}
                </button>
                {onCancel && (
                    <button type="button" className="secondary" onClick={onCancel}>
                        Close
                    </button>
                )}
            </div>
            {error && <p className="form-error">{error}</p>}
        </form>
    );
}