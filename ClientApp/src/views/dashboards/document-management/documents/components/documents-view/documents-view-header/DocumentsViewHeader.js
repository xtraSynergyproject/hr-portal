import { Box } from '@mui/material'
import DocumentsBreadcrumbs from './documents-breadcrumbs/DocumentsBreadcrumbs'
import SearchDocuments from './search-documents/SearchDocuments'

const DocumentsViewHeader = () => (
  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-end', flex: 1 }}>
    <DocumentsBreadcrumbs />
    <SearchDocuments />
  </Box>
)

export default DocumentsViewHeader;
