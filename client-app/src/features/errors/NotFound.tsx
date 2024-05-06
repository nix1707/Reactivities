import { Link } from "react-router-dom"
import { Button, Header, Icon, Segment } from "semantic-ui-react"

const NotFound = () => {
  return (
    <Segment placeholder>
        <Header icon>
            <Icon name='search'/>
            Oops - we've looked everywhere but couldn't find what are you looking for!
        </Header>
        <Segment.Inline>
            <Button as={Link} to='/activities'>
                Return to activities page
            </Button>
        </Segment.Inline>
    </Segment>
  )
}

export default NotFound