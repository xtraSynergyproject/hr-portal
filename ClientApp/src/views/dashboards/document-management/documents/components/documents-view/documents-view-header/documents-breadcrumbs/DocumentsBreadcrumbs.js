import Typography from '@mui/material/Typography'
import Breadcrumbs from '@mui/material/Breadcrumbs'
import Link from '@mui/material/Link'
import NavigateNextIcon from '@mui/icons-material/NavigateNext'

const DocumentsBreadcrumbs = () => {
  function handleClick(event) {
    event.preventDefault()
    console.info('You clicked a breadcrumb.')
  }
  return (
    <div role='presentation' onClick={handleClick}>
      <Breadcrumbs separator={<NavigateNextIcon fontSize='small' />} aria-label='breadcrumb'>
        <Link underline='hover' color='inherit' href='/'>
          My Workspace
        </Link>
        <Link underline='hover' color='inherit' href='/material-ui/getting-started/installation/'>
          HR Management Issues
        </Link>
        <Typography color='text.primary'>Test</Typography>
      </Breadcrumbs>
    </div>
  )
}

export default DocumentsBreadcrumbs
