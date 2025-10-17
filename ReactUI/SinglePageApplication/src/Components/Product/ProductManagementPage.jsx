import React, { useCallback, useEffect, useMemo, useState } from 'react';
import {
    Button,
    Card,
    CardHeader,
    makeStyles,
    shorthands,
    Spinner,
    Subtitle1,
    Text
} from '@fluentui/react-components';
import { AddRegular, ArrowClockwiseRegular } from '@fluentui/react-icons';
import ProductForm from './ProductForm.jsx';
import ProductTable from './ProductTable.jsx';
import {
    deleteProduct, fetchProducts, baseApiUrl, addProduct, editProduct
} from '../../Services/productService';

const deleteProductApiUrl = baseApiUrl + '/api/product/delete';
const productsApiUrl = baseApiUrl + '/api/product/list';
const addProductApiUrl = baseApiUrl + '/api/product/create';
const editProductApiUrl = baseApiUrl + '/api/product/edit';

const useStyles = makeStyles({
    layout: {
        display: 'grid',
        gridTemplateColumns: '1fr',
        gap: '24px'
    },
    formCard: {
        ...shorthands.padding('16px'),
        display: 'flex',
        flexDirection: 'column',
        rowGap: '12px'
    },
    tableCard: {
        ...shorthands.padding('0px')
    },
    toolbar: {
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        ...shorthands.padding('16px'),
        borderBottom: '1px solid #f0f0f0'
    },
    content: {
        ...shorthands.padding('16px')
    }
});

const ProductManagementPage = () => {
    const styles = useStyles();
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [feedback, setFeedback] = useState(null);
    const [submitting, setSubmitting] = useState(false);
    const [selectedProduct, setSelectedProduct] = useState(null);
    const [deletingId, setDeletingId] = useState(null);

    const isEditing = useMemo(() => Boolean(selectedProduct), [selectedProduct]);

    const loadProducts = useCallback(async () => {
        if (!productsApiUrl) {
            setError('The PRODUCTS_API_URL environment variable is not configured.');
            setLoading(false);
            return;
        }

        try {
            setLoading(true);
            setError('');
            const items = await fetchProducts(productsApiUrl);
            setProducts(Array.isArray(items) ? items : []);
        } catch (err) {
            setError(err.message || 'Unable to load products');
        } finally {
            setLoading(false);
        }
    }, [productsApiUrl]);

    useEffect(() => {
        loadProducts();
    }, []);

    const handleSubmit = async (product) => {
        if (!addProductApiUrl) {
            setFeedback({ type: 'danger', message: 'The ADD_PRODUCT_API_URL environment variable is not configured.' });
            return;
        }

        if (!editProductApiUrl) {
            setFeedback({ type: 'danger', message: 'The EDIT_PRODUCT_API_URL environment variable is not configured.' });
            return;
        }

        setFeedback(null);
        try {
            if (isEditing) {
                if (!product.id) {
                    setFeedback({ type: 'danger', message: 'The selected product does not have a valid identifier.' });
                    return;
                }
                const updatedProduct = await editProduct(editProductApiUrl, product);

                setProducts((prev) => {
                    return prev.map((item) => (item.id === selectedProduct.id ? { ...item} : item));
                });
                setFeedback({
                    type: 'success',
                    message: `\`${updatedProduct?.title ?? product.title}\` was updated successfully.`
                });
                
            }
            else {
                setSubmitting(true);
               
                const createdProduct = await addProduct(addProductApiUrl, product);

                setProducts((prev) => {
                    return [ ...prev];
                });
                setFeedback({
                    type: 'success',
                    message: `\`${createdProduct?.title ?? payload.title}\` was added successfully.`
                });
            }
            setSelectedProduct(null);
        } catch (err) {
            setError(err.message || 'Unable to save product');
        } finally {
            setSubmitting(false);
        }
    };

    const handleEdit = (product) => {
        setSelectedProduct(product);
    };

    const handleCancelEdit = () => {
        setSelectedProduct(null);
    };

    const handleDelete = async (product) => {
        const confirmDelete = window.confirm(
            `Are you sure you want to delete "${product.title}"?`
        );
        if (!confirmDelete) {
            return;
        }

        setDeletingId(product.id);
        setError('');
        try {
            const response = await fetch(`${deleteProductApiUrl}/${product.id}`, {
                method: 'DELETE'
            });

            if (!response.ok) {
                throw new Error('Failed to delete product');
            }

            setProducts((prev) => prev.filter((item) => item.id !== product.id));

            if (selectedProduct && selectedProduct.id === product.id) {
                setSelectedProduct(null);
            }
        } catch (err) {
            setError(err.message || 'Unable to delete product');
        } finally {
            setDeletingId(null);
        }
    };

    return (
        <div className={styles.layout}>
            <Card className={styles.formCard}>
                <CardHeader
                    header={<Subtitle1>{isEditing ? 'Edit product' : 'Add a new product'}</Subtitle1>}
                    description={
                        <Text role="presentation">
                            {isEditing
                                ? 'Update the product details and save your changes.'
                                : 'Enter the information for a new product to add it to the catalog.'}
                        </Text>
                    }
                />
                <ProductForm
                    key={selectedProduct ? selectedProduct.id : 'new'}
                    initialValues={
                        selectedProduct || {
                            title: '',
                            price: '',
                            description: ''
                        }
                    }
                    onSubmit={handleSubmit}
                    onCancel={isEditing ? handleCancelEdit : undefined}
                    submitting={submitting}
                    isEditing={isEditing}
                />
            </Card>

            <Card className={styles.tableCard}>
                <div className={styles.toolbar}>
                    <Text weight="semibold">Cataloge</Text>
                    <div>
                        <Button
                            icon={<ArrowClockwiseRegular />}
                            appearance="secondary"
                            onClick={loadProducts}
                            disabled={loading}
                        >
                            Refresh
                        </Button>
                        <Button
                            icon={<AddRegular />}
                            appearance="primary"
                            onClick={() => setSelectedProduct(null)}
                            style={{ marginLeft: '8px' }}
                        >
                            Add new
                        </Button>
                    </div>
                </div>
                <div className={styles.content}>
                    {loading ? (
                        <Spinner label="Loading products" />
                    ) : error ? (
                        <Text role="alert" appearance="danger">
                            {error}
                        </Text>
                    ) : (
                        <ProductTable
                            products={products}
                            onEdit={handleEdit}
                            onDelete={handleDelete}
                            deletingId={deletingId}
                        />
                    )}
                </div>
            </Card>
        </div>
    );
};

export default ProductManagementPage;