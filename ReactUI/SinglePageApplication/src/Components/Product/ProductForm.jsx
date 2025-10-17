import React, { useEffect, useState } from 'react';
import { Button, Field, Input, Textarea } from '@fluentui/react-components';

const ProductForm = ({ initialValues, onSubmit, onCancel, submitting, isEditing }) => {
  const [formValues, setFormValues] = useState(initialValues);

  useEffect(() => {
    setFormValues(initialValues);
  }, [initialValues]);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormValues((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    const formattedValues = {
      ...formValues,
      price: Number(formValues.price)
    };
    onSubmit(formattedValues);
  };

  return (
    <form onSubmit={handleSubmit} style={{ display: 'grid', gap: '12px' }}>
      <Field label="Title" required>
        <Input
          name="title"
          value={formValues.title}
          onChange={handleChange}
          required
          placeholder="Enter product name"
        />
      </Field>

      <Field label="Price" required>
        <Input
          name="price"
          type="number"
          min="0"
          step="0.02"
          value={formValues.price}
          onChange={handleChange}
          required
          placeholder="Enter price"
        />
      </Field>

      <Field label="Description">
        <Textarea
          name="description"
          value={formValues.description}
          onChange={handleChange}
          placeholder="Describe the product"
          resize="vertical"
        />
      </Field>

      <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '8px' }}>
        {onCancel ? (
          <Button type="button" appearance="secondary" onClick={onCancel} disabled={submitting}>
            Cancel
          </Button>
        ) : null}
        <Button appearance="primary" type="submit" disabled={submitting}>
          {isEditing ? 'Save changes' : 'Add product'}
        </Button>
      </div>
    </form>
  );
};

export default ProductForm;