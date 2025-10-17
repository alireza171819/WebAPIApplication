import React from 'react';
import {
  Button,
  Link,
  Table,
  TableBody,
  TableCell,
  TableHeader,
  TableHeaderCell,
  TableRow,
  Text
} from '@fluentui/react-components';
import { DeleteRegular, EditRegular } from '@fluentui/react-icons';

const ProductTable = ({ products, onEdit, onDelete, deletingId }) => {
  if (!products.length) {
    return <Text>No products available yet. Try adding one above.</Text>;
  }

  return (
    <Table aria-label="Product list">
      <TableHeader>
        <TableRow>
          <TableHeaderCell>Name</TableHeaderCell>
          <TableHeaderCell>Description</TableHeaderCell>
          <TableHeaderCell>Price</TableHeaderCell>
          <TableHeaderCell>Actions</TableHeaderCell>
        </TableRow>
      </TableHeader>
      <TableBody>
        {products.map((product) => (
          <TableRow key={product.id}>
            <TableCell>{product.title}</TableCell>
            <TableCell>${Number(product.price).toFixed(2)}</TableCell>
            <TableCell style={{ maxWidth: '260px' }}>
              <Text truncate>{product.description}</Text>
            </TableCell>
            <TableCell>
              <Button
                icon={<EditRegular />}
                appearance="secondary"
                onClick={() => onEdit(product)}
              >
                Edit
              </Button>
              {onDelete ? (
                <Button
                  icon={<DeleteRegular />}
                  appearance="transparent"
                  onClick={() => onDelete(product)}
                  disabled={deletingId === product.id}
                  style={{ marginLeft: '4px' }}
                >
                  Delete
                </Button>
              ) : null}
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
};

export default ProductTable;